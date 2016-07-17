public class PlayerCard : CharacterCard{

    //a class for cards used with player avatars
    //

    public PlayerCard(string name, Card.Factions faction):
        base(name, 0, 0, faction, null, 0, 0, 0, 0){}

    public override string Text{
        get{
            string t = "";
            t += Owner.ManaPerTurn.ToString() + " ink per turn\n";
            for(int f = 0; f < Owner.Loyalty.Length; ++f){
                if(Owner.Loyalty[f] > 0){
                    t += Owner.Loyalty[f].ToString() + " " + Card.FactionNames[f] + "\n";
                }
            }
            t += Owner.Hand.Length.ToString() + " cards in hand";
            return t;
        }
    }
    
}
