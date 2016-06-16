using UnityEngine;
using System.Collections.Generic;
using System;

public class Board : MonoBehaviour {

    static GameObject spacePrefab;

    void Awake() {
        if (spacePrefab == null) {
            spacePrefab = (GameObject) Resources.Load("Prefabs/Space");
        }
    }

    public void Set(Space.Types[,] t, int[,] m, int[,] v, int[,] h, Space.Effects[,] e = null) {
        //a pseudo constructor
        if(e==null) e  = new Space.Effects[t.GetLength(0),t.GetLength(1)];
        if (t.GetLength(0) != m.GetLength(0) || t.GetLength(0) != v.GetLength(0) || t.GetLength(0) != h.GetLength(0) || t.GetLength(0) != e.GetLength(0) ||
                t.GetLength(1) != m.GetLength(1) || t.GetLength(1) != v.GetLength(1) || t.GetLength(1) != h.GetLength(1) || t.GetLength(1) != e.GetLength(1)){
            throw new GameDataException("Board dimensions do not match across parametes.");
        }

        for (int i = 0; i < t.GetLength(0); ++i) {
            for (int j = 0; j < t.GetLength(1); ++j) {
                GameObject g = Instantiate(spacePrefab);
                g.GetComponent<Space>().Set(t[i, j], m[i, j], v[i, j], h[i,j], e[i,j]);
                GetComponent<Board>().AddSpace(g.GetComponent<Space>(), new Vector2(i, j) );
            }
        }
    }
    

    protected void AddSpace(Space s, Vector2 pos) {
        s.transform.parent = transform;
        s.transform.localPosition = pos;
    }

    public void AddBoard(Board b, Vector2 pos) {
        foreach(Space s in b.GetComponentsInChildren<Space>() ){
            Destroy(GetSpaceAt(pos+(Vector2)s.transform.localPosition));
            AddSpace(s, pos+(Vector2)s.transform.localPosition);
        }
    }

    public int[] CalcOverlap(Board b, Vector2 offset) {
        /* returns {matching overlap, non-matching overlap} in terms of numbers of tiles.
        Matching is defined as same type or both same vision and movement.*/
        int[] r = new int[2];
        foreach(Space s in b.GetComponentsInChildren<Space>()) {
            Space sp = GetSpaceAt(((Vector2)s.transform.localPosition + offset));
            if(sp == null) continue;
            if( s.Type == sp.Type || ( s.Move == sp.Move && s.Vision == sp.Vision)) {
                ++r[0];
            } else {
                ++r[1];
            }
        }
        return r;
    }

    public Space GetSpaceAt(Vector2 pos) {
        foreach (Space s in GetComponentsInChildren<Space>()) {
            if ((Vector2)s.transform.localPosition == pos) {
                Debug.Log("Found the space!");
                return s;
            }
        }
        return null;
    }

    public Space[] GetSpaces(Vector2 from, Vector2 to) {
        //return all spaces in the specified range (exclusive)
        List<Space> r = new List<Space>();
        Vector2 def = to - from;
        int i = 0; //track number of itterations
        //implements with a primitve algorythms that only works on cardinal and semi-cardinal directions
        Vector2 step = new Vector2((int)(def.x / (def.x==0?1:Math.Abs(def.x))), (int)(def.y / (def.y==0?1:Math.Abs(def.y))));
        /**/Debug.Log("Finding spaces in direction "+step.x.ToString()+","+step.y.ToString());
        for (Vector2 v = from+step; v != to; v += step) {
            r.Add(GetSpaceAt(v));
            if (i > 1000) throw new SearchException(); //prevent going to infinity
            ++i;
        }
        return r.ToArray();
    }

}
