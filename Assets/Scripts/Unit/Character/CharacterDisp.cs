using UnityEngine;
using System.Collections.Generic;

public class CharacterDisp : UnitDisp {

    static string[] statNames = { "Attack", "Range", "Defense" };
    static string[] attackTypeNames = { "Melee", "Ranged", "Magic" };
    
    protected override string spriteDir{
        get{ return "Art/OGA/Characters/"; }
    }

    SpriteRenderer at; //the attack type disp
    TextMesh status;
    Character c;
    Dictionary<CharacterCard.Stats, TextMesh> stats;

    protected virtual string wounds {
        get { return (c.IsWounded ? "W" : "");  }
    }

    protected virtual string attack {
        get { return (c.AttackAct ? "A" : "");  }
    }

    public new void Start() {
        base.Start();
        status = GetComponentInChildren<TextMesh>();
        c = GetComponent<Character>();
        at = StartAttackType();
        stats = StartStats();
	}
	
    protected virtual SpriteRenderer StartAttackType(){
        return transform.Find("StatBar/AttackType").GetComponent<SpriteRenderer>();
    }

    protected virtual Dictionary<CharacterCard.Stats, TextMesh> StartStats() {
        var stats = new Dictionary<CharacterCard.Stats, TextMesh>();
        for (int i = 0; i < 3; ++i) {
            stats.Add((CharacterCard.Stats)i, transform.Find("StatBar/" + statNames[i]).GetComponent<TextMesh>());
        }
        return stats;
    }

    void Update() {
        //continually update status
        status.text = (c.card.Owner == GameController.Game.ActivePlayer ? attack + (c.MoveAct ? "M" : "") : "") + wounds;
    }

	public void Disp() {
        //updates the display
        base.Disp();
        if(at != null) at.sprite = Resources.Load<Sprite>("Art/OGA/AttackTypes/" + attackTypeNames[(int)c.card.AttackType]); //bad code
        foreach(var key in stats.Keys) {
            stats[key].text = c.GetStat(key).ToString();
        }
    }

}
