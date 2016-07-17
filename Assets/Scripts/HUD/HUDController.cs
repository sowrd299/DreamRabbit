using UnityEngine;

public class HUDController : MonoBehaviour {

    public static ResourceSet<Sprite> FactionIcons;

    public GameController game;
    HandDisp hand;
    ManaDisp mana;
    LoyaltyDisp loyalty;
    CardDisp cardZoom;

    void Start() {
        FactionIcons = new ResourceSet<Sprite>("Art/Factions", Card.FactionNames);
        game = GameController.Game;
        hand = GetComponentInChildren<HandDisp>();
        mana = GetComponentInChildren<ManaDisp>();
        loyalty = GetComponentInChildren<LoyaltyDisp>();
        // testing
        cardZoom = GetComponentInChildren<CardDisp>();
        // /testing
        ClearZoom();
    }

	void Update() {
        hand.Disp(game.ActivePlayer.Hand, game.ActivePlayer.CanPlay);
        hand.DispStatus(InputController.LocalInput.Mode == InputController.Modes.NONE, game.ActivePlayer.HandType); 
        mana.Disp(game.ActivePlayer.Mana, game.ActivePlayer.ManaPerTurn );
        loyalty.Disp(game.ActivePlayer.Loyalty);
    }

    public void Zoom( Card c ){
        //displays the given card on the Card Zoom
        cardZoom.gameObject.SetActive(true);
        cardZoom.Disp(c);
    }

    public void ClearZoom(){
        cardZoom.gameObject.SetActive(false);
    }
	
}
