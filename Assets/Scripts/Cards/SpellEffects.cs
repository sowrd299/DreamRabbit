using UnityEngine;
using System;
using System.Reflection;

public static class SpellEffects {

    //a class for storing methods used as spell effects

    public static Delegate GetMethod(Type t, string name){
        return Delegate.CreateDelegate(t, typeof(SpellEffects).GetMethod(name));
    }

    //PREDICATES
    public static bool Character(Vector2 space) {
        //for targeting any character
        return GameController.Game.GetAtPos<Character>(space) != null;
    }

    public static bool Wounded(Vector2 space) {
        //for targeting wounded units
        Character c = GameController.Game.CharAtPos(space);
        return c != null && c.IsWounded;
    }

    public static bool WoundedOnDead(Vector2 space) {
        //for targeting wounded units standing on deadland
        return Wounded(space) && IsSpaceType(space, Space.Types.DEAD);
    }

    private static bool IsSpaceType(Vector2 space, Space.Types type){
        //a template effect, to be called by other effects
        //returns true if the target space is of the correct type
        return GameController.Game.board.GetSpaceAt(space).Type == type;
    }

    //TARGET EFFECTS
    public static void ConjureFog(Vector2 space) {
        //target space
        //add fog at that space
        GameController.Game.board.GetSpaceAt(space).GetComponent<Space>().SetEffect(Space.Effects.FOG);
    }

    public static void SpillMiasma(Vector2 space) {
        //target unit
        //destroy it, and put miasma at that space
        GameController.Game.CharAtPos(space).GetComponent<Character>().Kill();
        GameController.Game.board.GetSpaceAt(space).SetEffect(Space.Effects.MIASMA);
    }
    
        //stat buff effects
    private static void StatBuff(Vector2 space, CharacterCard.Stats stat, int val) {
        //a templated effect, to be called by other effects
        //give target unit a stat buff
        GameController.Game.GetAtPos<Character>(space).ApplyBuff(stat, val);
    }

    public static void Ferocity(Vector2 space) {
        StatBuff(space, CharacterCard.Stats.ATK, 2);
    }

    public static void ViolentMadness(Vector2 space) {
        StatBuff(space, CharacterCard.Stats.ATK, 2);
        StatBuff(space, CharacterCard.Stats.DEF, 2);
    }


    //AXIS EFFECTS
    public static void Resources(Player p, Space s) {
        p.GainManaPerTurn(1);
        p.GainLoyalty((Card.Factions)s.Type + 1, 1);
    }

    public static void AltResources(Player p) {
        p.GainManaPerTurn(1);
        p.GainMana(1);
    }

    public static void Pillar(Player p, Space s) {
        GameObject go = (GameObject)GameObject.Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/Pillar"), s.transform.position, new Quaternion() );
        go.GetComponent<Pillar>().Set(p);
    }

}
