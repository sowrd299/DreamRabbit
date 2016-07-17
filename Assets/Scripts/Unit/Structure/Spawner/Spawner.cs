using UnityEngine;
using System.Collections;

public class Spawner : CardStructure, IUnit {

    SpawnerCard card;
    public UnitCard Card {
        get { return card; } // testing/
    }

    public override Player Owner{
        get { 
            //**/Debug.Log("Using Spawner.Owner property override.");
            return Card.Owner; 
        }
    }

    protected override int Radius{
        get { return 0; }
    }

    public override Card Ability{
        get { return this.card.Ability; }
    }

    public override bool CanActivate(){
        return (GameController.Game.GetAtPos<Character>(transform.position) == null &&
                base.CanActivate());
    }

    public void Set( SpawnerCard card ){
        this.card = card;
        GetComponent<SpawnerDisp>().Start();
        GetComponent<SpawnerDisp>().Disp();
    }

}
