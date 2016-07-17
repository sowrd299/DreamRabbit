using UnityEngine;

public class SpaceDisp : MonoBehaviour {

    TextMesh tm;
    Space s;
    SpriteRenderer sr;
    public GameObject fog;

    static string spritesFolder = "Art/OGA/Terrain/";
    static string[] sprites = { "Dirt", "Water", "Grass", "Snow" };

    static private string typeNames = "DWNC";
    static private string moveNames = " mM";
    static private string visionNames = " vV";
    static private string heightNames = " hH";

    public void Start() {
        tm = GetComponentInChildren<TextMesh>();
        s = GetComponent<Space>();
        sr = GetComponentInChildren<SpriteRenderer>();
        fog = transform.Find("FogSprite").gameObject;
    }

    public void Update() {
        if (GetComponent<Space>() == null) {
            Destroy(gameObject);
        }
    }

	public void Disp() {
        tm.text = StatText(s); 
        sr.sprite = Resources.Load<Sprite>(spritesFolder + sprites[(int)s.Type]);
        fog.SetActive(s.Fog || s.Miasma);
        fog.GetComponent<SpriteRenderer>().color = s.Miasma ? new Color(0.2f, 1, 0) : new Color(1,1,1);
    }

    public static string StatText( Space s ){
        return new string( new char[] { moveNames[s.Move], visionNames[s.Vision], heightNames[s.Height], s.Fog? 'F':' ', s.Miasma?'T':' ' } );
    }

    public static string Text( Space s ){
        return typeNames[(int)s.Type] + StatText(s);
    }

}
