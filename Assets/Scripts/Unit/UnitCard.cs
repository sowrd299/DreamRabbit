using UnityEngine;
using System.Collections;
using System.Xml;

public abstract class UnitCard : TargetCard {

    //a class for cards that place an individual game object

    protected abstract string prefab{
        get;
    }

    public UnitCard( XmlNode xml, Player p ):
        base(xml, p) {}

    public UnitCard( string name, int cost, int loyalty, CharacterCard.Factions faction, Player p ):
        base(name, cost, loyalty, faction, p) {}

    protected override void target ( Vector2 target ){
        GameObject go = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Character"));
        go.transform.position = target;
        set( go );
    }

    protected abstract void set ( GameObject go );
    
}
