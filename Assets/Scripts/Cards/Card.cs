using UnityEngine;
using System.Xml;
using System;

public delegate bool TargetPred(Vector2 space);
public delegate void Target(Vector2 space);

public abstract class Card {

    public delegate void Exhaust(Card c);

    public enum Factions { NONE, EIR, SALT, WHITE, DRA}

    private int cost;
    public int Cost {
        get { return cost; }
    }

    private int loyalty;
    public int Loyalty {
        get { return loyalty; }
    }

    private string name;
    public string Name {
        get { return name;  }
    }

    private Factions faction;
    public Factions Faction {
        get { return faction; }
    }

    Player owner;
    public Player Owner {
        get { return owner; }
        set { if(owner == null) owner = value;  }
    }

    //xml constructor (and factory)
    public static Card Create(XmlNode xml, Player p){
        //exists to be inhereted
        //intended to be hidden by children
        //cannot be abstract b/c its children
        return null;
    }

    protected Card(XmlNode xml, Player p)
        :this(xml.Attributes["name"].Value,
             int.Parse(xml.Attributes["cost"].Value),
             int.Parse(xml.Attributes["loyalty"].Value),
             (Factions)int.Parse(xml.Attributes["faction"].Value), p) {}
    
    public Card(string name, int cost, int loyalty, Factions faction, Player owner) {
        this.name = name;
        this.cost = cost;
        this.loyalty = loyalty;
        this.faction = faction;
        this.owner = owner;
    }

    public abstract bool Play(InputController i, Exhaust e, Vector2 from, int radius);

}
