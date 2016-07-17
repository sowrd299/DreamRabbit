using UnityEngine;

public class UnitDisp : MonoBehaviour {

    SpriteRenderer s;
    private IUnit u;

    protected virtual string spriteDir{
        get{ return "Art/Units/"; }
    }

	public void Start () {
        s = GetComponentInChildren<SpriteRenderer>();
        u = GetComponent<IUnit>();
    }

    public virtual void Disp(){
        //**/ Debug.Log( spriteDir + u.Card.Name );
        s.sprite = Resources.Load<Sprite>(spriteDir + u.Card.Name);
    }
    
}
