using UnityEngine;

public class PillarDisp : MonoBehaviour {

    static readonly float[] activeAlphas = { 0.5f, 1f };

	void Update () {
        GetComponentInChildren<SpriteRenderer>().color = new Color( 1f,1f,1f,GetComponent<Pillar>().Owner == InputController.LocalInput.ActivePlayer? activeAlphas[1] : activeAlphas[0]);	
	}
    
}
