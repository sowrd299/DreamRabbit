using UnityEngine;
using System.Collections;
using System.Xml;

public abstract class UnitCard : TargetCard {

    //a class for cards that place an individual game object

    protected override string imageDir {
        get { return "Art/Units/"; }
    }

    protected abstract string prefab{
        get;
    }

    public UnitCard( XmlNode xml, Player p ):
        base(xml, p) {}

    public UnitCard( string name, int cost, int loyalty, CharacterCard.Factions faction, Player p ):
        base(name, cost, loyalty, faction, p) {}

    protected override void target ( Vector2 target ){
        GameObject go = Object.Instantiate(Resources.Load<GameObject>(prefab));
        go.transform.position = target;
        set( go );
    }

    protected abstract void set ( GameObject go );

    public override bool Play (InputController i, Card.Exhaust e, Vector2 v, int radius){
        if(radius > 0) radius -= 1;
        return base.Play(i,e,v,radius);
    }
    
}
