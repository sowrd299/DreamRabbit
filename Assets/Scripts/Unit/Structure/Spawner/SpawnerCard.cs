using UnityEngine;
using System.Xml;

public class SpawnerCard : UnitCard {
    
    const string cardDir = "Assets/Resources/Data/Cards.xml";
    static readonly CardLoader loader = new XmlCardLoader(cardDir);
    protected override string prefab{
        get{ return "Prefabs/Spawner"; }
    }

    private CharacterCard ability;
    public CharacterCard Ability{
        get{ return ability; }
    }

    public SpawnerCard( XmlNode xml, Player p):
        base(xml, p)
    {
        this.ability = loader.Load(xml.Attributes["Ability"].Value, p) as CharacterCard;
    }
        

    protected override void set( GameObject go ){
        go.GetComponent<Spawner>().Set(this);
    }

}
