using System.Numerics;
using HumanDash.Enum;

namespace HumanDash.Entities;

public class Avatar : Entity
{
    protected int _nbLife;
    protected AvatarState State{get; set;}
    protected Vector2 Position{get; set;}
    protected double Speed{get; set;}
    protected bool IsAlive{get;}
    protected int NbLife{get; set;}
//contructeur de avatar
    public Avatar(){
    }
//commencer son saut avatar
    protected bool startJump(){
        bool start = false;
        if(State == AvatarState.Idle || State == AvatarState.Running ){
            start  =true;
            //State = AvatarState.Jumping;
        }
        else{
            start = false;
        }
        return start;
    }
    //descendre ou annuler son saut 
    protected bool cancelJump(){
        bool cancelJ = false;
        if(State == AvatarState.Jumping){
            cancelJ = true;
        }
        return cancelJ;
    }
    //se lever après glisse
    protected bool getUp(){
        bool glisse = false;
        if(State == AvatarState.Sliding){
            glisse = true;
        }
        else{
            glisse = false;
        }
        return glisse;
    }
    //descendre
    protected bool drop(){
        
        return false;
    }
    //aucune idee pourquoi canard *_*
    protected bool duck(){
        return false;

        bool drop = false;
        if(State == AvatarState.Jumping){
            drop = true;
        }
        else{
            drop = false;
        }
        return drop;
       

    }
    
    //meurt
    protected bool die(){
        bool life = false;
        if(_nbLife <=0){
            life = true;
            // State = AvatarState.Idle;
        }
        else{
            life = false;
        }
        return life;
    }

    
}