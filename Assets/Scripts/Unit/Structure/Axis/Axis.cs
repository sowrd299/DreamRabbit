using UnityEngine;

public class Axis : Structure {

    public virtual Card[] GetHand(Player p) {
        //return an array of the axis powers granted
        return new Card[] {
            //come up with better names
            new AxisCard("Mana and Loyalty", 0, p, this, SpellEffects.Resources),
            new AxisCard("More Mana", 0, p, this, (Player _p, Space s) => SpellEffects.AltResources(_p)),
            new AxisCard("Pillar", 5, p, this, SpellEffects.Pillar)
        };
    }

    public virtual void Exhaust(Card c) {
        //to be called once the axis has been used, for axis power c
        Destroy(gameObject);
    }

}
