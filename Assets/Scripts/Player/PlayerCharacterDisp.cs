using UnityEngine;
using System.Collections.Generic;

public class PlayerCharacterDisp : CharacterDisp {

    protected override string wounds {
        //displays wounds as non-binary state
        get {
            return GetComponent<Character>().Wounds.ToString();
        }
    }

    protected override string attack {
        get {
            return "";
        }
    }

    protected override SpriteRenderer StartAttackType() {
        return null;
    }

    protected override Dictionary<CharacterCard.Stats, TextMesh> StartStats() {
        //only displays deffense
        var stats = new Dictionary<CharacterCard.Stats, TextMesh>();
        stats.Add(CharacterCard.Stats.DEF, transform.Find("StatBar/Defense").GetComponent<TextMesh>());
        return stats;
    }

}
