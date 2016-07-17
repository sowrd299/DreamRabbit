using UnityEngine;

delegate void PlayerEffect(Player p);

class PlayerEffectCard : Card {

    //a class for cards that effect (based on the) player who cast them

    PlayerEffect effect;

    public PlayerEffectCard(string name, int cost, int loyalty, Factions faction, Player owner, PlayerEffect effect, string text = "") :
        base(name, cost, loyalty, faction, owner, text)
    {
        this.effect = effect;
    }

    public override bool Play(InputController i, Exhaust e, Vector2 from = default(Vector2), int radius = 0) {
        effect(Owner);
        e(this);
        return true;
    }

}
