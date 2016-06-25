using UnityEngine;
using System.Xml;
using System.Collections.Generic;
using System;

public class TerrainCard : TargetCard {

    //a card type for playing terrain onto the board

    Space.Types[,] types;
    Space.Effects[,] effects;
    Vector2[] axes;
    Dictionary<Space.Stats,int[,]> stats;
    
    public Board Board {
        get {
            //turns the terrain created with the card is played
            Board b = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Board")).GetComponent<Board>();
            b.Set(types, stats[Space.Stats.MOVE], stats[Space.Stats.VISION], stats[Space.Stats.HEIGHT], effects);
            /**/b.gameObject.SetActive(false);
            return b;
        }
    }

    public static new TerrainCard Create(XmlNode xml, Player p) {
        return new TerrainCard(xml, p);
    }

    protected TerrainCard(XmlNode xml, Player p):
        base(xml,p)
    {
        string[] statNames = {"move","vision","height"};
        int w = int.Parse(xml.Attributes["w"].Value);
        int h = int.Parse(xml.Attributes["h"].Value);
        //stats
        stats = new Dictionary<Space.Stats,int[,]>();
        for(int i = 0; i < statNames.Length; ++i){
            stats.Add((Space.Stats)i, toGridFromXml(w,h,xml,statNames[i],(int j)=>j)); 
            //**/Debug.Log("Loaded "+statNames[i]+" for "+xml.Attributes["name"].Value);
        }
        //types
        types = toGridFromXml(w,h,xml,"type",(int i)=>(Space.Types)i);
        //effects
        effects = toGridFromXml(w,h,xml,"space_effect",(int i)=>(Space.Effects)i);
        //axes
        List<Vector2> a = new List<Vector2>(); //testing
        if(xml.Attributes["axes"] != null){
            for(int i = 0; i < w; ++i){
                for(int j = 0; j < h; ++j){
                    if(xml.Attributes["axes"].Value[j*w+i] == '1'){
                        a.Add(new Vector2(i,h-1-j));
                    }
                }
            }
        }
        axes = a.ToArray();        
    }

    private static T[,] toGridFromXml<T>(int w, int h, XmlNode xml, string name, Converter<int,T> c){
        //a wrapper for toGrid that reads from xml
        //produces a blank grid if string not in xml
        if(xml.Attributes[name] != null){
            return toGrid(w,h,xml.Attributes[name].Value,c);
        }   
        //**/Debug.Log("Did not find string for "+name);
        return new T[w,h];
    }

    private static T[,] toGrid<T>(int w, int h, string s, Converter<int,T> c){
        //converts a one-dimensional string to a grid
        //move to helper methods class
        if(s.Length != w*h) throw new ArgumentException("Length of string does not match dimensions of grid.");
        T[,] r = new T[w,h];
        for(int i = 0; i < s.Length; ++i){
            r[i%w,h-1-(i/w)] = c(s[i]-'0');
        }
        return r;
    }

    public TerrainCard(string name, int cost, int loyalty, Factions faction, Player owner, Space.Types[,] types, int[,] move = null, int[,] vision = null, int[,] height = null, Vector2[] axes = null) :
        base(name, cost, loyalty, faction, owner)
    {
        //out of date
        this.types = types;
        stats = new Dictionary<Space.Stats,int[,]>();
        stats.Add(Space.Stats.MOVE, move);
        stats.Add(Space.Stats.VISION, vision);
        stats.Add(Space.Stats.HEIGHT, height);
        stats = populateStats(stats, types.GetLength(0), types.GetLength(1));
        if (axes != null){
            this.axes = axes;
        } else {
            this.axes = new Vector2[0];
        }
    }

    private Dictionary<Space.Stats,int[,]> populateStats(Dictionary<Space.Stats,int[,]> stats, int w, int h){
        //~depricated
        foreach(Space.Stats s in Enum.GetValues(typeof(Space.Stats))){
            if(!stats.ContainsKey(s) || stats[s] == null){
                stats.Remove(s);
                stats.Add(s,new int[w,h]);
            };
        }
        return stats;
    }

    protected override bool targetPred( Vector2 target ){
        Board b = Board;
        int[] overlap = GameController.Game.board.CalcOverlap(b, target);
        //**/Debug.Log("Overlap is " + overlap[0].ToString() + "," + overlap[1].ToString() );
        GameObject.Destroy(b.gameObject);
        if (overlap[0] > 0 && overlap[1] <= overlap[0]) {
            return true;
        }
        return false;
    }

    protected override void target( Vector2 target ){
        GameController.Game.placeBoard(Board, target); 
        placeAxes( target );
    }

    private void placeAxes( Vector2 target ){
        foreach( Vector2 loc in axes ){
            if(GameController.Game.StructureAtPos(loc+target) == null){
                GameController.Game.PlaceAxis(target+loc);
            }
        }
    }

}
