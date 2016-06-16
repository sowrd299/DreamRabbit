using UnityEngine;
using System;
using System.Reflection;

public static class SpellEffects {

    //a class for storing methods used as spell effects

    public static Delegate GetMethod(Type t, string name){
        return Delegate.CreateDelegate(t, typeof(SpellEffects).GetMethod(name));
    }

    //PREDICATES
    public static bool Wounded(Vector2 space) {
        //for targeting wounded units
        Character c = GameController.Game.CharAtPos(space);
        return c != null && c.IsWounded;
    }

    //TARGET EFFECTS
    public static void ConjureFog(Vector2 space) {
        //target space
        //add fog at that space
        GameController.Game.board.GetSpaceAt(space).GetComponent<Space>().SetEffect(Space.Effects.FOG);
    }

    public static void SpillMiasma(Vector2 space) {
        //target wounded unit
        //destroy it, and put miasma at that space
        GameController.Game.CharAtPos(space).GetComponent<Character>().Kill();
        GameController.Game.board.GetSpaceAt(space).SetEffect(Space.Effects.MIASMA);
    }

    //AXIS EFFECTS
    public static void Resources(Player p, Space s) {
        p.GainManaPerTurn(1);
        p.GainLoyaly((Card.Factions)s.Type + 1, 1);
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
