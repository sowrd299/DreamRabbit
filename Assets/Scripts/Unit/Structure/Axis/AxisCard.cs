using UnityEngine;

delegate void AxisEffect(Player p, Space s);

class AxisCard : PlayerEffectCard {

    //a class for the powers granted by Axis

    Axis axis;

    public AxisCard(string name, int c, Player owner, Axis axis, AxisEffect effect) :
        base(name, c, 0, Factions.NONE, owner, (Player p) => effect(p, GameController.Game.board.GetSpaceAt(axis.transform.position)))
    {
        this.axis = axis;
    }

    public override bool Play(InputController i, Exhaust e, Vector2 from = default(Vector2), int radius = 0) {
        return base.Play(i, (Card c)=>{ e(c); axis.Exhaust(c);}, from, radius);
    }

}
