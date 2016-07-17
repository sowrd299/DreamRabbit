using UnityEngine;
using System.Collections;

public class Pillar : Structure {

    Player owner;
    public Player Owner{
        get { return owner; }
    }

    public virtual void Set(Player owner){
        //pseudo-constructor
        this.owner = owner;
    }

    public virtual void Capture(Player owner){
        //to be called when captured
        this.owner = owner;
     }

    public virtual void Turn(){
        //to be called once a trun
    }

    public virtual int getValue(Player p){
        return p == Owner? 1:0;
    }

}
