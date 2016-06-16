using UnityEngine;
using System.Collections.Generic;
using System;

public class Character : MonoBehaviour, IUnit {

    const int movesPerTurn = 2; //the number of spaces a unit can move each turn by default

    //stores status effects on an instance of a character

    public CharacterCard card;
    public Card Card{
        get { return card; }
    }

    Dictionary<CharacterCard.Stats, int> buffs;

    private int wounds;
    public virtual int Wounds {
        get { return wounds + (GameController.Game.board.GetSpaceAt(transform.position).Miasma?1:0); }
    }
    public virtual bool IsWounded {
        get { return Wounds > 0; }
    }
    public virtual bool IsDead {
        get { return Wounds > 1;  }
    }

    //store what a unit has done this turn;
    /*
    0 - can move/attack
    1 - has alread this turn
    2 - maynot this or next turn
    */
    private int moveAct;
    public bool MoveAct {
        get { return moveAct < movesPerTurn; } //may move twice per turn
    }
    private int attackAct;
    public bool AttackAct {
        get { return attackAct == 0; }
    }
    Vector2 facing;

    public void Set(CharacterCard card) {
        this.card = card;
        ClearBuffs();
        wounds = 0;
        GetComponent<CharacterDisp>().Start();
        GetComponent<CharacterDisp>().Disp();
    }

    public void Turn() {
        //once-per-turn upkeep
        ClearBuffs();
        if (moveAct > 0) moveAct -= movesPerTurn; //may move twice per turn
        if (moveAct < 0) moveAct = 0; 
        if(attackAct > 0) --attackAct;
    }

    public void ApplyBuff(CharacterCard.Stats stat, int i) {
        //add a value to a stat
        buffs[stat] += i;
    }

    public void ClearBuffs() {
        //removes all buffs from the character
        buffs = new Dictionary<CharacterCard.Stats, int>();
        for(int i = 0; i < 3; ++i) {
            buffs.Add((CharacterCard.Stats)i, 0);
        }
    }

    public int GetStat(CharacterCard.Stats stat) {
        //return the currect value of a stat, with buffs
        return card.Stat[stat] + buffs[stat];
    }

    //MOVEMENT
    public virtual bool CanMove(Space s) {
        //return true if can legally mvoe into space
        //move considered legal if space is adjecent, not a barrier and unoccupied
        return MoveAct && s.Move < 2 && Math.Abs(s.transform.position.x - transform.position.x) <= 1 &&
                Math.Abs(s.transform.position.y - transform.position.y) <= 1;
    }

    public virtual void Move(Space s) {
        //moves the character to the given space
        facing = s.transform.position - transform.position; //update facing
        transform.position = s.transform.position;
        if (IsDead) Die();
    }

    public virtual void ExhaustMove(Space s) {
        //to be called after the character moves normally
        moveAct += 1 + s.Move;
    }

    //COMBAT
    public virtual bool CanAttack(Character c) {
        //determines in the given character is a legal attack target
        //test has attack action
        if (!AttackAct) return false;
        //test not friendly
        if (c.card.Owner == card.Owner) return false;
        //test visible
        if (GameController.Game.board.GetSpaceAt(c.transform.position).Vision > 1) return false;
        //test in attack type's patern
        Vector2 dif = c.transform.position - transform.position;
        switch (card.AttackType) {
            case CharacterCard.AttackTypes.MELEE: //may only attack forward
                if (Math.Abs(Vector2.Angle(facing, dif)) != 0 && Math.Abs(Vector2.Angle(facing, dif)) != 45) {
                    return false;
                }
                break;
            case CharacterCard.AttackTypes.RANGED: //may only attack in straight line
                if (dif.x != 0 && dif.y != 0) {
                    return false;
                }
                break;
            case CharacterCard.AttackTypes.MAGIC: //may only attack on diagnals
                if (Math.Abs(dif.x) != Math.Abs(dif.y)) {
                    return false;
                }
                break;
        }
        //test vision
        //*
        foreach(Space s in GameController.Game.board.GetSpaces(transform.position, c.transform.position)){
            if (s != null && s.Vision > 0) return false;
        }//*/
        //test within range
        if(Math.Abs(dif.x) > GetStat(CharacterCard.Stats.RNG) || Math.Abs(dif.y) > GetStat(CharacterCard.Stats.RNG)) {
            return false;
        }
        return true;
    }

    public virtual void Attack(Character c) {
        Debug.Log(card.Name + " attacked " + c.card.Name);
        int atkVal = AtkVal(c);
        int defVal = c.DefVal(this);
        if (atkVal > defVal + 1) {
            c.Kill();
        }else if(atkVal >= defVal - 1) {
            c.Wound();
        }
    }

    public int AtkVal(Character c) {
        //return the attack value used in attacking character c
        return GetStat(CharacterCard.Stats.ATK) + GameController.Game.board.GetSpaceAt(transform.position).Advantage;
    }

    public int DefVal(Character c) {
        //return the defense value used in defending against character c
        return GetStat(CharacterCard.Stats.DEF);
    }

    public virtual void ExhaustAttack() {
        //to be called after 
        ++attackAct;
        if (MoveAct) moveAct = movesPerTurn;
    }


    //WOUNDS, DEATH AND DYING
    public virtual bool Kill() {
        return Wound() && Wound();
    }

    public virtual bool Wound() {
        ++wounds;
        if (IsDead) Die();
        return true;
    }

    public virtual void Die() {
        Destroy(gameObject);
    }

}
