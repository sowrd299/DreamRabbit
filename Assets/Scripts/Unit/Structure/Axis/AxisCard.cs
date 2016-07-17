using UnityEngine;

delegate void AxisEffect(Player p, Space s);

class AxisCard : PlayerEffectCard {

    //a class for the powers granted by Axis

    protected override string imageDir{
        get{ return base.imageDir + "Axis/"; }
    }

    Axis axis;

    public AxisCard(string name, int c, Player owner, Axis axis, AxisEffect effect, string text = "", int loyalty = 0, Factions faction = Factions.NONE) :
        base(name, c, loyalty, faction, owner, (Player p) => effect(p, GameController.Game.board.GetSpaceAt(axis.transform.position)), text)
    {
        this.axis = axis;
    }

    public override bool Play(InputController i, Exhaust e, Vector2 from = default(Vector2), int radius = 0) {
        return base.Play(i, (Card c)=>{ e(c); axis.Exhaust(c);}, from, radius);
    }

}
