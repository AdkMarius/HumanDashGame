using HumanDash.Enum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = System.Numerics.Vector2;

namespace HumanDash.Entities;

public class Avatar : IGameEntity
{
    private const int RUNNER_IDLE_POSX = 247;
    private const int RUNNER_IDLE_POSY = 8;
    private const int RUNNER_WIDTH = 58;
    private const int RUNNER_HEIGHT = 73;
    
    protected int _nbLife;
    protected AvatarState State {get; set;}
    protected Vector2 Position {get; set;}
    protected double Speed {get; set;}
    protected bool IsAlive {get;}
    protected int NbLife {get; set;}
    
    private Sprite _idleSprite;
    private Sprite _runSprite;
    
    //contructeur de avatar
    public Avatar(Texture2D spriteSheet, Vector2 position, SoundEffect jumpingSound)
    {
        Position = position;
        _idleSprite = new Sprite(
            spriteSheet,
            RUNNER_IDLE_POSX,
            RUNNER_IDLE_POSY,
            RUNNER_WIDTH,
            RUNNER_HEIGHT
        );
        State = AvatarState.Idle;
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

    public int DrawOrder { get; set; }
    public void Update(GameTime gameTime)
    {
        throw new System.NotImplementedException();
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (State == AvatarState.Idle)
        {
            _idleSprite.Draw(spriteBatch, Position);
        }
    }
}