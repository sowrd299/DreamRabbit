using UnityEngine;

public class OldTerrainCard : Card {

    //DEPRICATED
    //a card type for playing terrain onto the board

    Space.Types[,] types = new Space.Types[3, 3];
    int[,] move;
    int[,] vision;
    int[,] height;
    int[,] effect;

    public Board Board {
        get {
            //turns the terrain created with the card is played
            Board b = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Board")).GetComponent<Board>();
            b.Set(types, move, vision, height);
            /**/b.gameObject.SetActive(false);
            return b;
        }
    }

    public OldTerrainCard(string name, int cost, int loyalty, Factions faction, Player owner, Space.Types[,] types, int[,] move = null, int[,] vision = null, int[,] height = null) :
        base(name, cost, loyalty, faction, owner)
    {
        this.types = types;
        if(move != null) {
            this.move = move;
        } else {
            this.move = new int[types.GetLength(0), types.GetLength(1)];
        }
        if(vision != null) {
            this.vision = vision;
        } else {
            this.vision= new int[types.GetLength(0), types.GetLength(1)];
        }
        if (height != null) {
            this.height = height;
        } else {
            this.height = new int[types.GetLength(0), types.GetLength(1)];
        }
    }

    public override bool Play(InputController i, Exhaust ex, Vector2 from = default(Vector2), int radius = 0) {
        ex(this); //will be changed once I get around to overhauling terrain cards
        return i.Set(InputController.Modes.PLACE_BOARD, Board.gameObject);
        
    }

}
