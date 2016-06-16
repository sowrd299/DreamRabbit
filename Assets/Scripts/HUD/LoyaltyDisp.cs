using UnityEngine;
using UnityEngine.UI;

public class LoyaltyDisp : MonoBehaviour {

    Text[] texts;
    GameObject[] icons;

    void Start() {
        texts = GetComponentsInChildren<Text>();
    }

	public void Disp(int[] values) {
        for(int i = 0; i < texts.Length; ++i) {
            transform.GetChild(i).gameObject.SetActive(values[i + 1] != 0);
            texts[i].text = values[i + 1].ToString();
        }
    }
}
