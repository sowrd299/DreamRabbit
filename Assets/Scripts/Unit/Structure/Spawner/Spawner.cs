using UnityEngine;
using System.Collections;

public class Spawner : CardStructure, IUnit {

    public Card Card {
        get { return null; } // testing/
    }

    protected override int Radius{
        get { return 0; }
    }

    public override bool CanActivate(){
        return (GameController.Game.GetAtPos<Character>(transform.position) == null &&
                base.CanActivate());
    }

}
