using UnityEngine;
using System;

public class InputController : MonoBehaviour {
    
    public static InputController LocalInput {
        get { return GameObject.FindGameObjectWithTag("GameController").GetComponent<InputController>();  }
    }

    HUDController hud;

    public enum Modes { OFF, NONE, MOVE_CHAR, PLACE_BOARD, PLACE_CHAR, TARGET }
    /* off: inactive/unreceptive
     * none: ready for new command
     * move character: interact with a character
     * target: select target for a card
     */

    //public for unity
    public float panSpeed; //the speed at witch to move the camera

    Modes mode = Modes.NONE;
    public Modes Mode {
        get {return mode;}
    }
    //mode-specific vars
    //only used while is some modes
    GameObject placing;
    TargetPred targetPred;
    Target target;
    Character active;

    //cursors
    GameObject cursor;
    GameObject activeCursor;

    private Player activePlayer;
    public Player ActivePlayer{
        //the player currently sitting at the keyboard
        //(as distinguished from the player who's turn it is, as stored in game controller)
        get { return activePlayer; }
        set{ activePlayer = value; /*bad*/ mode = Modes.NONE; /*/bad*/}
    }

	void Start () {
        cursor = transform.Find("Cursor").gameObject;
        activeCursor = Instantiate(cursor);
        hud = GameObject.FindWithTag("HUD").GetComponent<HUDController>();
    }
	
	void Update () {
        //what was clicked on/moused over:
        Space space = GameController.Game.board.GetSpaceAt(mouseSpace());
        Character character = GameController.Game.CharAtPos(mouseSpace());
        CardStructure cardStructure = GameController.Game.GetAtPos<CardStructure>(mouseSpace(), "Spawner");
        // shit code, please improve
        //handle the cursor
        cursor.SetActive(false);
        {
            GameObject _cursor = placing == null ? cursor : placing ;
            _cursor.SetActive(true);
            _cursor.transform.position = mouseSpace();
        }
        activeCursor.SetActive(active != null);
        if(active != null) {
            activeCursor.transform.position = active.transform.position;
        }
        //cursor color/icon
        //*
        {
            string s = "Cursor";
            switch (mode) {
                case Modes.MOVE_CHAR:
                    if (space != null) {
                        if (active != null) {
                            if (character != null && active.CanAttack(character)) {
                                s = "Attack";
                            } else if (character == null && active.CanMove(space)) {
                                s = "Move";
                            }
                        }
                    }
                    break;
                case Modes.TARGET:
                    if(targetPred != null && targetPred(mouseSpace())) {
                        s = "Target";
                    }
                    break;
            }
            cursor.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Art/Cursors/" + s);
        }//*/
        // /shit code
        //handle clicks
        if (Input.GetMouseButtonDown(0)) {
            switch (mode) {
                //control a character
                case Modes.MOVE_CHAR:
                    if (active != null) {
                        //move if did not click on character
                        if (character == null) {
                            if (active.CanMove(space)) {
                                active.Move(space);
                                active.ExhaustMove(space);
                                if (!active.MoveAct) Clear();
                            }
                        }
                        //attack if did
                        else {
                            if (active.CanAttack(character)) {
                                active.Attack(character);
                                active.ExhaustAttack();
                                if (!active.MoveAct) Clear();
                            }
                        }
                    }
                    break;
                //places the loaded board piece where clicked
                case Modes.PLACE_BOARD:
                    if (placing != null && GameController.Game.placeBoard(placing.GetComponent<Board>(), mouseSpace())) {
                        /**/Debug.Log("Placing terrain.");
                        Destroy(placing);
                        Clear();
                    }
                    break;
                //places the loaded character where clicked
                case Modes.PLACE_CHAR:
                    if(placing != null && targetPred(mouseSpace())) {
                        Clear();
                    }
                    break;
                //handle things that want to target spaces
                case Modes.TARGET:
                    if (targetPred(mouseSpace())) {
                        target(mouseSpace());
                        Clear();
                    }
                    break;
            } 
        }else if (Input.GetMouseButtonDown(1)) {
            //select character
            switch (mode) {
                case Modes.NONE:
                    if(cardStructure != null && ActivePlayer == cardStructure.Owner && cardStructure.CanActivate()){
                        //above conditions will bug w/ non-spawner CardStructures that can active under a character
                        /**/Debug.Log("Activating a CardStructure");
                        cardStructure.Activate();
                        break;
                    } else {
                        mode = Modes.MOVE_CHAR;
                        goto case Modes.MOVE_CHAR;
                    }
                case Modes.MOVE_CHAR:
                    active = character;
                    if(active == null) {
                        mode = Modes.NONE;
                    } else {
                        //select the clicked on character
                        hud.Zoom(active.Card);
                        if (active.card.Owner != ActivePlayer){
                            active = null;
                            mode = Modes.NONE;
                        }
                        else Debug.Log("Have selected " + active.card.Name);
                    }
                    break;
            }
        }
        //use number keys to play cards
        if (mode == Modes.NONE) {
            for (int i = 0; i < ActivePlayer.Hand.Length && i < 10; ++i) {
                if (Input.GetKeyDown("" + i) || (i == 0 && Input.GetKeyDown("`"))) { // acccepts ` as an alterative to 0
                    hud.Zoom(ActivePlayer.Hand[i]);
                    if (Input.GetKey(KeyCode.Tab)){
                        //**/Debug.Log("Playing a " + ActivePlayer.Hand[i].GetType().ToString());
                        ActivePlayer.Play(ActivePlayer.Hand[i], this);
                    }
                }
            }
        }
        //pan control with WASD
        if (Input.GetKey(KeyCode.A)) {
            Camera.main.transform.position += new Vector3(-panSpeed, 0);
        }else if(Input.GetKey(KeyCode.D)) {
            Camera.main.transform.position += new Vector3(panSpeed, 0);
        }else if(Input.GetKey(KeyCode.W)) {
            Camera.main.transform.position += new Vector3(0, panSpeed);
        }else if(Input.GetKey(KeyCode.S)) {
            Camera.main.transform.position += new Vector3(0, -panSpeed);
        }
        //end the turn on <space>
        if (Input.GetKeyDown(KeyCode.Space)) {
            mode = Modes.OFF;
            Clear();
            GameController.Game.Turn();
        }
        //abort input with <esc>
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (mode == Modes.PLACE_BOARD) Destroy(placing);
            Clear();
        }
	}

    public bool Set(Modes mode, GameObject placing = null, TargetPred tp = null) {
        //allows an ouside source to tell it what input mode to go into
        if (this.mode == Modes.NONE) {
            this.mode = mode;
            this.placing = placing;
            targetPred = tp;
            return true;
        }
        return false;
    }

    public bool Set(Modes mode, TargetPred tp = null, Target t = null) {
        if(this.mode == Modes.NONE) {
            this.mode = mode;
            targetPred = tp;
            target = t;
            return true;
        }
        return false;
    }

    protected void Clear() {
        //reset to default state
        hud.ClearZoom();
        mode = Modes.NONE;
        placing = null;
        active = null;
    }

    protected Vector2 mouseSpace() {
        //returns truncated verstion of the mouse possition
        Vector2 m = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2((float)Math.Floor(m.x+0.5f), (float)Math.Floor(m.y+0.5f));
    }
}
