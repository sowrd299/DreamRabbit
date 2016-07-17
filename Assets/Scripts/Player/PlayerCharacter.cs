using UnityEngine;
using System.Collections;

public class PlayerCharacter : Character {

    protected virtual int woundsToDead{
        get { return 4; }
    }

    public override bool IsDead {
        //shapers are imortal
        get { return false; }
    }

    public void Set(string name, Card.Factions faction, Player p, int def) {
        //allows the card to not need to be played
        //may move to intermitent "token character" class
        //also kinda depricated
        card = new CharacterCard(name, 0, 0, faction, p, 0, 0, 0, def);
    }

    public override bool CanAttack(Character c) {
        return false;
    }

    public override bool Wound(Character c = null) {
        //"dealling n+'th wound => pillar" game rule
        if(c != null && Wounds >= woundsToDead - 1 && GameController.Game.StructureAtPos(c.transform.position) == null){
            // weird code
            SpellEffects.Pillar(c.Card.Owner, GameController.Game.board.GetSpaceAt(c.transform.position));
            Wounds = 0;
            // /weird code
        }
        return base.Wound(c);
    }

}
