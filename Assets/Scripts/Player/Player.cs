using System.Collections.Generic;
using UnityEngine;

public class Player {

    const int baseManaPerTurn = 3;
    const int baseHandSize = 5;

    public PlayerCharacter avatar;

    private Deck deck;
    private List<Card> hand;
    public Card[] Hand {
        get {
            Axis a = GameController.Game.AxisAtPos(avatar.transform.position);
            if (a != null) return a.GetHand(this);
            return hand.ToArray();
        }
    }

    public HandDisp.Modes HandType {
        get{
            if(Hand.Length > 0 && Hand[0] is AxisCard) return HandDisp.Modes.AXIS;
            return HandDisp.Modes.NORM;
        }
    }

    private int mana;
    public int Mana {
        get { return mana; }
    }

    private int manaPerTurn;
    public int ManaPerTurn {
        get { return manaPerTurn;  }
    }

    private int[] loyalty;
    //stores loyalty to each faction in slot correlated to that faction
    //i.e. loyalty[1] stores loyalty to Eirmoore
    public int[] Loyalty {
        get { return loyalty; }
    }

    public int CastRadius {
        get { return 3-(avatar.Wounds>0?1:0); }
    }

    public Player(CharacterCard card, Vector2 pos, int[] loyalty, string deckUrl) {
        //cards
        deck = new Deck(deckUrl, new XmlCardLoader("Cards.xml"), this);
        hand = new List<Card>();
        //resources
        manaPerTurn = baseManaPerTurn;
        this.loyalty = loyalty;
        //avatar
        card.Owner = this;
        avatar = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Avatar")).GetComponent<PlayerCharacter>();
        avatar.Set(card);
        avatar.transform.position = pos;
    }
    
    public void SetUp() {
        //draw the opening hand
        deck.Shuffle();
        for(int i = 0; i < baseHandSize; ++i){
            Draw();
        }
    }


    public void Turn() {
        //to be called once at the start of every turn
        //turn-base update method
        mana = manaPerTurn;
        Draw();
    }

    public virtual void RunTurn(){
        //to be called when it is time for the player's turn to happen
        //currently has code for local players (over network or a.i.) by default
        InputController.LocalInput.ActivePlayer = this;
    }

    public bool Draw() {
        //add the top card of the deck to the hand
        Card c = deck.Draw();
        if(c != null){
            hand.Add(c);
            return true;
        }
        return false;
    }

    public void GainMana(int amount) {
        mana += amount;
    }

    public void GainManaPerTurn(int amount) {
        manaPerTurn += amount;
    }

    public void GainLoyaly(Card.Factions faction, int amount) {
        loyalty[(int)faction] += amount;
    }

    public bool CanPlay(Card c) {
        //states whether or not a card would be legal for the player to play
        /**/Debug.Log("Cost is "+c.Cost.ToString()+"; Lolayty is "+c.Loyalty.ToString()+" to "+c.Faction.ToString());
        return c.Cost <= Mana && loyalty[(int)c.Faction] >= c.Loyalty;
    }

    public bool Play(Card c, InputController ic) {
        //a wrapper for other functions that together play a card normally
        if (CanPlay(c) && c.Play(ic, ExhaustPlay, avatar.transform.position, CastRadius)) {
            return true;
        }
        return false;
    }

    public void ExhaustPlay(Card c) {
        GainMana(-c.Cost);
        Discard(c);
    }

    public bool Discard(Card c) {
        //discard a card
        if (hand.Contains(c)) {
            hand.Remove(c);
            return true;
        }
        return false;
    }

}
