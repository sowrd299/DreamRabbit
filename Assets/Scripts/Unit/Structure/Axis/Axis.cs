using UnityEngine;

public class Axis : Structure {

    Space.Types spaceType;

    private Card[] hand;
    public virtual Card[] GetHand(Player p) {
        //return an array of the axis powers granted
        if(hand == null || hand[0].Owner != p || GameController.Game.board.GetSpaceAt(p.avatar.transform.position).Type != spaceType){
            //refresh the hand
            hand = makeHand(p);
        }
        return hand;
    }

    private Card[] makeHand(Player p){
        spaceType = GameController.Game.board.GetSpaceAt(p.avatar.transform.position).Type;
        return new Card[] {
            //come up with better names
            new AxisCard("Ink & Loyalty", 0, p, this, SpellEffects.Resources, "Gain 1 ink per turn.\nGain 1 loyalty."),
            new AxisCard("Fresh Ink", 0, p, this, (Player _p, Space s) => SpellEffects.AltResources(_p), "Gain 1 ink.\nGain 1 ink per turn"),
            new AxisCard("Pillar", 5, p, this, SpellEffects.Pillar, "This Inkwell becomes a Pillar."),
            getFactionCard(spaceType, p)
        };
    }

    private Card getFactionCard(Space.Types t, Player p){
        //returns the special axis card ascociated with the given space
        switch(t){
            case Space.Types.DEAD:
                return new AxisCard("Devotion", 0, p, this, (Player q, Space s) => q.GainLoyalty(Card.Factions.EIR, 2), "Gain 2 loyalty.", 2, Card.Factions.EIR);
            case Space.Types.WATER:
                return new AxisCard("Ink Stream", 0, p, this, (Player q, Space s) => q.GainMana(3), "Gain 3 ink.", 2, Card.Factions.SALT);
            case Space.Types.NATURE:
                return new AxisCard("Shroud", 0, p, this, (Player q, Space s) => SpellEffects.ConjureFog(s.transform.position), "Give this space fog.", 2, Card.Factions.WHITE);
            case Space.Types.COLD:
                return new AxisCard("Survey", 2, p, this, (Player q, Space s) => q.Draw(2), "Draw 2 cards.", 3, Card.Factions.DRA);
        }
        return null;
    }

    public virtual void Exhaust(Card c) {
        //to be called once the axis has been used, for axis power c
        Destroy(gameObject);
    }

}
