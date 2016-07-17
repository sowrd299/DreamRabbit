using UnityEngine;
using UnityEngine.UI;

public class CardDisp : MonoBehaviour { 

    /*a class for displaying cards in card form on the screen
     */

    GameObject axisIcon;
    Image crest;
    Text cost;
    Text loyalty;
    Text name;
    Image art;
    Text text;

    public virtual void Start(){
        axisIcon = transform.Find("AxisIcon").gameObject;
        crest = transform.Find("Crest").GetComponent<Image>();
        cost = transform.Find("Cost").GetComponent<Text>();
        loyalty = transform.Find("Loyalty").GetComponent<Text>();
        name = transform.Find("Name").GetComponent<Text>();
        art = transform.Find("Art").GetComponent<Image>();
        text = transform.Find("Text").GetComponent<Text>();
    }

    public virtual void Disp( Card c ){
        axisIcon.SetActive( c is AxisCard );
        crest.sprite = HUDController.FactionIcons[(int)c.Faction];
        cost.text = c.Cost.ToString();
        loyalty.text = c.Loyalty.ToString();
        name.text = c.Name;
        art.sprite = c.Image;
        text.text = c.Text;
    }

}
