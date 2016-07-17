using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class CharacterCard : UnitCard {

    //store fixed individual stats of a character

    protected override string prefab {
        get{ return "Prefabs/Character"; }
    }

    protected override string imageDir {
        get { return base.imageDir + "Characters/"; }
    }

	public enum AttackTypes { MELEE, RANGED, MAGIC }
    public static readonly string[] AttackTypeNames = { "Melee", "Ranged", "Twisted" };
    public enum Stats { ATK, RNG, DEF }

    AttackTypes attackType;
    public AttackTypes AttackType {
        get { return attackType; }
    }

    Dictionary<Stats, int> stats;
    public Dictionary<Stats, int> Stat {
        get { return stats; }
    }

    public override string Text{
        get {
            //maybe should not permimently add this to text...
            return base.Text + "\n" + 
                    Stat[Stats.ATK].ToString() + "-" + 
                    Stat[Stats.RNG].ToString() + "-" +
                    AttackTypeNames[(int)AttackType] + "-" + 
                    Stat[Stats.DEF].ToString();
        }
    }

    //xml contstructor
    public static new Card Create(XmlNode xml, Player p){
        return new CharacterCard(xml, p);
    }

    private CharacterCard(XmlNode xml, Player p):
        base(xml, p)
    {
        init((AttackTypes)int.Parse(xml.Attributes["attackType"].Value),
                int.Parse(xml.Attributes["atk"].Value),
                int.Parse(xml.Attributes["rng"].Value),
                int.Parse(xml.Attributes["def"].Value));
    }

    public CharacterCard(string name, int cost, int loyalty, Factions faction, Player owner, AttackTypes type, int atk, int rng, int def):
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


    protected override void set( GameObject go ){
        /**/Debug.Log("Setting the character.");
        go.GetComponent<Character>().Set(this);
    }

}
