using UnityEngine.UI;
using UnityEngine;

public class TextHandDisp : HandDisp {

    static readonly Color[] modeColors = { new Color(1f,1f,1f), new Color(144f/255, 195f/255, 212f/255) };
    static readonly float[] activeAlphas = { 0.5f, 1f };

    Text t;

    void Start() {
        t = GetComponent<Text>();
    }

    public override void Disp(Card[] hand) {
        string s = "";
        for(int i = 0; i < hand.Length; ++i) {
            s += "\n" + i.ToString() + ":" + hand[i].Name;
        }
        t.text = s;
    }

    public override void DispStatus(bool active, Modes mode){
        Color c = modeColors[(int)mode];
        t.color = new Color(c.r,c.g,c.b,active? activeAlphas[1] : activeAlphas[0]);
    }

}
