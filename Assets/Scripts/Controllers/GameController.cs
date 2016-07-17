using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    protected GameStats stats{
        get { return GameStats.Current; }
    }

    public static GameController Game {
        get { return GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();  }
    }

    public Board board;

    // testing
    Space.Types[,] typesMap = { { Space.Types.NATURE, Space.Types.WATER, Space.Types.DEAD },
                                { Space.Types.WATER, Space.Types.WATER, Space.Types.DEAD},
                                { Space.Types.DEAD, Space.Types.COLD, Space.Types.DEAD } };
    // /testing

    int[,] moveMap = new int[3, 3];
    int[,] visionMap = new int[3, 3];
    int[,] heightMap = new int[3, 3];

    Player[] players{
        get { return stats.Players; }
    }

    int activePlayer;
    public Player ActivePlayer {
        get { return players[activePlayer]; }
    }

    void Start() {
        board.Set(typesMap, moveMap, visionMap, heightMap);
        activePlayer = 0;
        // testing
        Player[] players = new Player[]{
                new Player(new PlayerCard("Sailor", Card.Factions.SALT) ,new Vector2(0,1),new int[] {0,2,2,0,0}, "Saltsan.csv"),
                new Player(new PlayerCard("WalkingRabbit", Card.Factions.WHITE) ,new Vector2(2,1),new int[] {0,0,0,3,0}, "Whitemist.csv") };
        GameStats stats = new GameStats(players);
        stats.MakeCurrent();
        //GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>().Set((new XmlCardLoader("Cards.xml")).Load("Rabbit", players[1]), players[1]);
        // /testing
        foreach(Player p in players){
            p.SetUp();
        }
        Turn();
    }
	
    public void Turn() {
        //advances to the next turn
        ++activePlayer;
        activePlayer %= players.Length;
        //calls all necessary turn functions
        ActivePlayer.Turn();
        //call turn for all owned characters
        foreach(var go in GameObject.FindGameObjectsWithTag("Character")) {
            Character c = go.GetComponent<Character>();
            if (c.card.Owner == ActivePlayer) {
                c.Turn();
            }
        }
        ActivePlayer.RunTurn();
        //check for the winner
        if(stats.Winner != null){
            SceneManager.LoadScene("EndGame");
        }
    }

    public bool placeBoard(Board b, Vector2 pos) {
        int[] overlap = board.CalcOverlap(b, pos);
        /**/Debug.Log("Overlap is " + overlap[0].ToString() + "," + overlap[1].ToString() );
        if (overlap[0] > 0 && overlap[1] <= overlap[0]) {
            Debug.Log("Placing board.");
            board.AddBoard(b, pos);
            return true;
        }
        return false;
    }

    public bool PlaceAxis( Vector2 pos ){
        //place a new axis at pos, remove any overriden structures
        if(StructureAtPos(pos) != null) Destroy(StructureAtPos(pos).gameObject);
        GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Axis"));
        go.transform.position = pos;
        return true;
    }

    public Character CharAtPos(Vector2 pos) {
        //depricated
        return GetAtPos<Character>(pos);
    }

    public Axis AxisAtPos(Vector2 pos) {
        //depricated
        return GetAtPos<Axis>(pos);
    }
        
    public Structure StructureAtPos(Vector2 pos){
        //manual specialization for GetAtPos 
        string[] tags = {"Axis", "Pillar", "Spawner"}; //maybe put someplace better?
        foreach(string s in tags){
            Structure st = GetAtPos<Structure>(pos,s);
            if(st != null) return st;
        }
        return null;
    }

    public T GetAtPos<T>(Vector2 pos, string tag = null) where T : MonoBehaviour {
        //consider moving to help methods class
        //forcefull maintain specialization:
        //if(tag == null && typeof(T) == typeof(Structure)) return GetAtPos(pos);
        if(tag == null) tag = typeof(T).Name;
        foreach (var c in GameObject.FindGameObjectsWithTag(tag)) {
            if ((Vector2)c.transform.position == pos) {
                return c.GetComponent<T>();
            }
        }
        return null;
    }
        

}
