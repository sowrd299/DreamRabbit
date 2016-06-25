using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class OldCharacterCard : TargetCard {

    //DEFUNCT
    //store fixed individual stats of a character

	public enum AttackTypes { MELEE, RANGED, MAGIC }
    public enum Stats { ATK, RNG, DEF }

    AttackTypes attackType;
    public AttackTypes AttackType {
        get { return attackType; }
    }

    Dictionary<Stats, int> stats;
    public Dictionary<Stats, int> Stat {
        get { return stats; }
    }

    //xml contstructor
    public static new Card Create(XmlNode xml, Player p){
        return new OldCharacterCard(xml, p);
    }

    private OldCharacterCard(XmlNode xml, Player p):
        base(xml, p)
    {
        init((AttackTypes)int.Parse(xml.Attributes["attackType"].Value),
                int.Parse(xml.Attributes["atk"].Value),
                int.Parse(xml.Attributes["rng"].Value),
                int.Parse(xml.Attributes["def"].Value));
    }

    public OldCharacterCard(string name, int cost, int loyalty, Factions faction, Player owner, AttackTypes type, int atk, int rng, int def):
        base(name, cost, loyalty, faction, owner)
    {
        init(type, atk, rng, def);
    }

    private void init (AttackTypes type, int atk, int rng, int def){
        //common body for constructors
        attackType = type;
        stats = new Dictionary<Stats, int>();
        stats.Add(Stats.ATK, atk);
        stats.Add(Stats.RNG, rng);
        stats.Add(Stats.DEF, def);
    }

    public override bool Play(InputController i, Exhaust ex, Vector2 from = default(Vector2), int radius = 0) {
        setParams(from, radius - 1);
        //create the character
        GameObject go = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Character"));
        //because the bellow line is commented, this code will not function
        //go.GetComponent<Character>().Set(this);
        ex(this); //ehaust
        //place
        i.Set(InputController.Modes.PLACE_CHAR, go, (Vector2 g)=>(targetPred(g) && GameController.Game.CharAtPos(g).gameObject == go));
        return true;
    }
}
