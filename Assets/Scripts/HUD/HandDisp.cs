using UnityEngine;

public abstract class HandDisp : MonoBehaviour {

    public enum Modes { NORM, AXIS };

    public abstract void Disp(Card[] hand);

    public abstract void DispStatus(bool active, Modes mode);
	
}
