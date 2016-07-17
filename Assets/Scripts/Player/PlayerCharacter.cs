using UnityEngine;
using System.Collections;

public class PlayerCharacter : Character {

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

}
