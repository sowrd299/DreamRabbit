using UnityEngine;
using System.Collections;

public class CardStructure : Structure{

    //A class for structures that can be activated to play a card

    protected const int baseRadius = 2;
    
    private Player owner;
    public virtual Player Owner{
        get { return owner; } //implementation may need improvement
    }

    private Card ability;
    public virtual Card Ability{
        get { return ability; }
    }

    protected virtual int Radius{
        get { return baseRadius; }
    }

    public void Set( Card ability, Player owner ){
        this.ability = ability;
        this.owner = owner;
    }
    
    public virtual bool CanActivate(){
        return Owner.CanPlay(Ability);
    }
        
    public virtual void Activate(){
        Ability.Play(InputController.LocalInput, Exhaust, transform.position, Radius);
    }

    protected virtual void Exhaust(Card c){
        //something tbd by game design
        owner.ExhaustPlay(c);
    }

}
