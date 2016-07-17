using UnityEngine;
using System.Xml;

public class SpawnerCard : UnitCard {
    
    const string cardDir = "Cards.xml";
    static readonly CardLoader loader = new XmlCardLoader(cardDir);

    public override string Text{
        get{ return "Spawns " + Ability.Name + "."; }
    }

    protected override string prefab{
        get{ return "Prefabs/Spawner"; }
    }

    private CharacterCard ability;
    public CharacterCard Ability{
        get{ return ability; }
    }

    public static new Card Create( XmlNode xml, Player p ){
        return new SpawnerCard( xml, p );
    }

    public SpawnerCard( XmlNode xml, Player p):
        base(xml, p)
    {
        this.ability = loader.Load(xml.Attributes["ability"].Value, p) as CharacterCard;
    }

    protected override void set( GameObject go ){
        go.GetComponent<Spawner>().Set(this);
    }

}
