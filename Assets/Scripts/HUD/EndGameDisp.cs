using UnityEngine;
using UnityEngine.UI;

public class EndGameDisp : MonoBehaviour {

	void Start () {

        Text t = GetComponentInChildren<Text>();
        Player p = GameStats.Current.Winner;
        /**/if(t != null && p != null) Debug.Log("Found all objects for endgame display.");
        t.text = "for " + p.Name;

	}
	
}
