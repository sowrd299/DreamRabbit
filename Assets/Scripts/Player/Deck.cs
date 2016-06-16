using System.Collections.Generic;
using System.IO;
using UnityEngine;

class Deck {

    //a class for storing a deck of cards, implemented as a list
    
    protected readonly static string dir = "Assets/Resources/Data/Decks/";

    private List<Card> cards;
    
    public Deck(string url, CardLoader loader, Player p):
        this(new StreamReader(dir+url), loader, p) {}

    public Deck(StreamReader file, CardLoader loader, Player p){
        cards = new List<Card>();
        Load(file,loader,p);
    }

    public void Load(StreamReader file, CardLoader loader,  Player p){
        string line;
        while ( (line = file.ReadLine()) != null ){
            string[] data = line.Split(',');
            for(int i = 0; i < int.Parse(data[1]); ++i){
                Card c = loader.Load(data[0], p);
                if(c != null) cards.Add(c);
                /**/else Debug.Log("Card "+data[0]+" failed to load to deck.");
            }
            /**/Debug.Log(cards[cards.Count-1].Name+" added to deck");
        }
    }

    public void Shuffle(){
        //randomized the order
        List<Card> newCards = new List<Card>();
        while(cards.Count > 0){
            Card c = cards[(int)(Random.value*(cards.Count-1))];
            newCards.Add(c);
            cards.Remove(c);
        }
        cards = newCards;
    }

    public Card Draw(){
        //pop the top card of the deck
        Card r = cards[cards.Count-1];
        cards.Remove(r);
        return r;
    }

}
