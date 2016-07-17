using UnityEngine;

public abstract class HandDisp : MonoBehaviour {
    
    public delegate bool CanPlay( Card c );

    public enum Modes { NORM, AXIS };

    public abstract void Disp(Card[] hand, CanPlay p);

    public abstract void DispStatus(bool active, Modes mode);
	
}
