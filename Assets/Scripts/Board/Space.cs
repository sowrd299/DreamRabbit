using UnityEngine;
using System.Collections.Generic;

public class Space : MonoBehaviour {

    public enum Types { DEAD, WATER, NATURE, COLD }
    readonly static int[] defMove = { 0, 1, 0, 1 };
    readonly static int[] defVision = { 0, 0, 1, 0 };
    readonly static int[] defHeight = { 0, 0, 0, 1 };

    Types type = Types.DEAD;
    public Types Type {
        get { return type;  }
    }

    public enum Effects { NONE, FOG, MIASMA, PURE }
    //miasma: units on miasma count as wounded
    //pure: unites on miasma loose all wounds; may never be implemented
    Effects effect;
    public Effects Effect {
        get { return effect; }
    }

    /*
    for move and vision:
    0 - no effect
    1 - stop (can enter but cannot go throw)
    2+ - block (cannot enter)
    */

    public enum Stats { MOVE, VISION, HEIGHT }
    public Dictionary<Stats,int> stats;

    public int Move {
        get { return stats[Stats.MOVE];  }
    }

    public int Vision {
        get { return stats[Stats.VISION]+ (Fog?1:0); }
    }

    public int Height {
        get { return stats[Stats.HEIGHT];  }
    }

    public bool Fog {
        get { return Effect == Effects.FOG; }
    }
    //fog=true increases effective vision by 1; also increases combat advantage
    
    public bool Miasma {
        get { return Effect == Effects.MIASMA; }
    }

    public int Advantage {
        //may or may not be depricated?
        //the number used to calculate bonuses confered to standy
        get { return Height>2?2:Height; }
    }
    
    public void Set(Types t, int m = 0, int v = 0, int h = 0, Effects effect = Effects.NONE ) {
        //m and v should be desigher value +1
        //give 0 for default value based on type
        type = t;
        stats = new Dictionary<Stats,int>();
        stats.Add(Stats.MOVE, m == 0 ? defMove[(int)t] : m-1);
        stats.Add(Stats.VISION, v == 0 ? defVision[(int)t] : v-1);
        stats.Add(Stats.HEIGHT, h == 0 ? defHeight[(int)t] : h-1);
        this.effect = effect;
        GetComponent<SpaceDisp>().Start();
        GetComponent<SpaceDisp>().Disp();
    }

    public void SetEffect(Effects val) {
        effect = val;
        GetComponent<SpaceDisp>().Disp(); 
    }

}
