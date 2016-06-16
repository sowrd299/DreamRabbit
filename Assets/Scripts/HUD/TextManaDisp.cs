using UnityEngine.UI;

public class TextManaDisp : ManaDisp {

    Text t;
    
    void Start() {
        t = GetComponentInChildren<Text>();
    }

    public override void Disp(int value, int outOf = 0) {
        t.text = value.ToString() + (outOf > 0 ? "/"+outOf.ToString() : "");
    }

}
