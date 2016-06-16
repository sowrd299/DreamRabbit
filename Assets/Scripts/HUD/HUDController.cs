using UnityEngine;

public class HUDController : MonoBehaviour {

    public GameController game;
    HandDisp hand;
    ManaDisp mana;
    LoyaltyDisp loyalty;

    void Start() {
        game = GameController.Game;
        hand = GetComponentInChildren<HandDisp>();
        mana = GetComponentInChildren<ManaDisp>();
        loyalty = GetComponentInChildren<LoyaltyDisp>();
    }

	void Update() {
        hand.Disp(game.ActivePlayer.Hand);
        hand.DispStatus(InputController.LocalInput.Mode == InputController.Modes.NONE, game.ActivePlayer.HandType); 
        mana.Disp(game.ActivePlayer.Mana, game.ActivePlayer.ManaPerTurn );
        loyalty.Disp(game.ActivePlayer.Loyalty);
    }
	
}
