using UnityEngine;

public class GameStats {

    /* a class for storing settings for a game
     */
    protected static GameStats current;
    public static GameStats Current{
        get { return current; }
    }

    protected const int baseVPtoWin = 3;
    public virtual int VPtoWin{
        get { return baseVPtoWin; }
    }

    private Player[] players;
    public virtual Player[] Players{
        get { return players; }
    }

    public GameStats( Player[] players ){
        this.players = players;
    }

    public void MakeCurrent(){
        current = this;
    }

    private Player winner;
    public Player Winner {
        get { 
            if(winner == null) winner = calcWinner();
            return winner; 
        }
    }

    public Player CalcWinner(){
        winner = calcWinner();
        return winner;
    }

    private Player calcWinner(){
        /**/Debug.Log("Checking for winner.");
        foreach(Player p in players){
            /**/Debug.Log("Player " + p.Name + " has " + p.VPs.ToString() + " VP's/");
            if(p.VPs >= VPtoWin){
                return p;
            }
        }
        return null;
    }

}
