using UnityEngine;
using System;
using System.Xml;

public class TargetCard : Card {

    //a card that applies and effect to a single target

    Target effect;
    Target Effect{
        get{
            if(effect != null){
                return effect;
            }
            return target;
        }
    }
    TargetPred pred; //additional prerequisits

    //things to store per play
    Vector2 from;
    protected int radius;

    // (prelimary) xml constructor and factor
    public static Card Create(XmlNode xml, Player p){
        return new TargetCard(xml, p);
    }

    protected TargetCard(XmlNode xml, Player p) :
        base(xml, p)
    {
        if(xml.Attributes["effect"] != null) effect = SpellEffects.GetMethod(typeof(Target), xml.Attributes["effect"].Value) as Target;
        if(xml.Attributes["pred"] != null) pred = SpellEffects.GetMethod(typeof(TargetPred), xml.Attributes["pred"].Value) as TargetPred;
    }

    public TargetCard(string name, int cost, int loyalty, Factions faction, Player owner, Target effect = null, TargetPred pred = null):
        base(name, cost, loyalty, faction, owner)
    {
        this.effect = effect;
        this.pred = pred;
    }

    protected virtual bool targetPred(Vector2 space) {
        //returns whether or not a target is considered legal
        //by default legal targets are defined as within target radius
        /**/Debug.Log("Casting from " + from.x.ToString() + "," + from.y.ToString());
        /**/Debug.Log("Casting at relative pos " + Math.Abs(space.x - from.x).ToString() + "," + Math.Abs(space.y - from.y));
        return (GameController.Game.board.GetSpaceAt(space) != null && Math.Abs(space.x - from.x) <= radius) && (Math.Abs(space.y - from.y) <= radius) && (pred!=null?pred(space):true);
    }

    protected virtual void target(Vector2 space){
        //pass
     }

    protected void setParams(Vector2 from, int radius) {
        //set parametes used by predicate
        this.from = from;
        this.radius = radius;
    }

    public override bool Play(InputController i, Exhaust ex, Vector2 from = default(Vector2), int radius = 0) {
        setParams(from, radius);
        /**/Debug.Log("Casting from " + from.x.ToString() + "," + from.y.ToString());
        return i.Set(InputController.Modes.TARGET, targetPred, (Vector2 s) => { ex(this); Effect(s); });
    }

}

