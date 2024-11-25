using System.Numerics;
using HumanDash.Enum;

namespace HumanDash.Entities;

public class Avatar : Entity
{
    protected int _nbLife;
    protected AvatarState State{get; set;}
    protected Vector2 Position{get; set;}
    protected double Speed{get; set;}
    protected bool IsAlive{get; set;}
    protected int NbLife{get; set;}
//contructeur de avatar
    public Avatar(){


    }
//commencer son saut avatar
    protected bool startJump(){
        return false;
    }
    //descendre ou annuler son saut 
    protected bool cancelJump(){
        return false;
    }
    //se lever
    protected bool getUp(){
        return false;
    }
    //se baisser
    protected bool drop(){
        return false;
    }
    //aucune idee pourquoi canard
    protected bool duck(){
        return false;
    }
    //meurt
    protected bool die(){
        bool life = false;
        if(_nbLife <=0){
            life = true;
        }
        else{
            life = false;
        }
        return life;
    }
}