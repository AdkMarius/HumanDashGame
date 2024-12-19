using System;
using HumanDash.Enum;
using HumanDash.Graphics;
using HumanDash.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = System.Numerics.Vector2;

namespace HumanDash.Entities;

public class Avatar : IGameEntity, ICollidable
{
    private const int AVATAR_IDLE_TEXTURE_POSX = 247;
    private const int AVATAR_IDLE_TEXTURE_POSY = 8;
    private const int AVATAR_IDLE_WIDTH = 58;
    private const int AVATAR_IDLE_HEIGHT = 73;

    private const int AVATAR_RUNNING_TEXTURE_POSX = 20;
    private const int AVATAR_RUNNING_TEXTURE_POSY = 10;
    private const int AVATAR_RUNNING_TEXTURE_WIDTH = 80;
    private const int AVATAR_RUNNING_TEXTURE_HEIGHT = 60;
    private const int NUMBER_OF_FRAME_IN_RUNNING_ANIMATION = 8;
    private const float RUNNING_ANIMATION_TIMESTAMP = 0.06f;

    private const int AVATAR_SLIDING_TEXTURE_POSX = 10;
    private const int AVATAR_SLIDING_TEXTURE_POSY = 10;
    private const int AVATAR_SLIDING_TEXTURE_WIDTH = 80;
    private const int AVATAR_SLIDING_TEXTURE_HEIGHT = 70;
    private const int NUMBER_OF_FRAME_IN_SLIDING_ANNIMATION = 5;
    private const float SLIDING_ANIMATION_TIMESTAMP = 0.2f;


    private const float JUMPING_VELOCITY = -680f;
    private const float CANCEL_JUMPING_VELOCITY = -100f;
    private const float GRAVITY = 1600f;
    private const float MINIMUM_JUMPING_VALUE = 40f;
    private const float DROP_VELOCITY = 300f;

    public const float START_GAME_SPEED = 300f;
    public const float MAX_SPEED = 1500f;
    private const float ACCELERATION_PPS_PER_SECOND = 5f;

    public Rectangle CollisionBox
    {
        get
        {
            Rectangle box = new Rectangle(
                (int)Math.Round(Position.X),
                (int)Math.Round(Position.Y),
                AVATAR_RUNNING_TEXTURE_WIDTH,
                AVATAR_RUNNING_TEXTURE_HEIGHT    
            );
            box.Inflate(-20, -5);
            return box;
        }
    }

    public AvatarState State {get; set;}
    
    private SoundEffect _jumpingSound;
    private SoundEffect _hitSound;
    
    public int DrawOrder { get; set; }

    public Vector2 Position {get; set;}
    public float Speed { get; private set; }
    public bool IsAlive { get; private set; }
    
    private float _verticalVelocity;
    private float _dropVelocity;
    private float _startRunnerPosY;
    
    private Sprite _idleSprite;
    private SpriteAnimation _runningAnimation;
    private SpriteAnimation _slidingAnimation;
    
    public event EventHandler JumpComplete;
    public event EventHandler Died;
    
    public Avatar(Texture2D slidingSpritesheet, Texture2D runningSpritesheet, Vector2 position, SoundEffect jumpingSound, SoundEffect hitObstacleSound)
    {
        Position = position;
        _idleSprite = new Sprite(
            slidingSpritesheet,
            AVATAR_IDLE_TEXTURE_POSX,
            AVATAR_IDLE_TEXTURE_POSY,
            AVATAR_IDLE_WIDTH,
            AVATAR_IDLE_HEIGHT
        );
        State = AvatarState.Idle;
        IsAlive = true;
        _jumpingSound = jumpingSound;
        _hitSound = hitObstacleSound;
        _startRunnerPosY = position.Y;

        _runningAnimation = new SpriteAnimation();
        CreateRunningAnimation(runningSpritesheet);
        _runningAnimation.Start();
        
        _slidingAnimation = new SpriteAnimation();
        CreateSlidingAnimation(slidingSpritesheet);
        _slidingAnimation.Start();
    }

    public bool StartJump()
    {
        if (State == AvatarState.Jumping || State == AvatarState.Falling)
            return false;

        _jumpingSound.Play();
        State = AvatarState.Jumping;
        _verticalVelocity = JUMPING_VELOCITY;
        return true;
    }

    
    public bool CancelJump()
    {
        if (State != AvatarState.Jumping || (_startRunnerPosY - Position.Y) < MINIMUM_JUMPING_VALUE)
        {
            return false;
        }

        _verticalVelocity = _verticalVelocity < CANCEL_JUMPING_VELOCITY ? CANCEL_JUMPING_VELOCITY : 0;
        
        return true;
    }

    public bool Slide()
    {
        if (State != AvatarState.Running)
        {
            return false;
        }
        
        State = AvatarState.Sliding;
        return true;
    }

    public bool GetUp()
    {
        if (State != AvatarState.Sliding)
            return false;

        State = AvatarState.Running;
        return true;
    }

    public bool Drop()
    {
        if (State != AvatarState.Falling && State != AvatarState.Jumping)
        {
            return false;
        }

        State = AvatarState.Falling;
        _dropVelocity = DROP_VELOCITY;
        return true;
    }
    
    
    public void Update(GameTime gameTime)
    {
        if (State == AvatarState.Jumping || State == AvatarState.Falling)
        {
            Position = new Vector2(Position.X, Position.Y + (_verticalVelocity + _dropVelocity) * (float)gameTime.ElapsedGameTime.TotalSeconds);
            _verticalVelocity += GRAVITY * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_verticalVelocity >= 0)
            {
                State = AvatarState.Falling;
            }
            
            if (Position.Y > _startRunnerPosY)
            {
                Position = new Vector2(Position.X, _startRunnerPosY);
                _verticalVelocity = 0;
                
                // much be AvatarState.Running
                State = AvatarState.Running;
                
                OnJumpComplete();
            }
        } else if (State == AvatarState.Running)
        {
            _runningAnimation.Update(gameTime);
        } else if (State == AvatarState.Sliding)
        {
            _slidingAnimation.Update(gameTime);
        }
        
        if (State != AvatarState.Idle)
        {
            Speed += ACCELERATION_PPS_PER_SECOND * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Speed > MAX_SPEED)
                Speed = MAX_SPEED;
            
        }

        _dropVelocity = 0;
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (State == AvatarState.Idle || State == AvatarState.Jumping || State == AvatarState.Falling)
        {
            _idleSprite.Draw(spriteBatch, Position);
        }
        else if (State == AvatarState.Running)
        {
            _runningAnimation.Draw(spriteBatch, Position);
        } else if (State == AvatarState.Sliding)
        {
            _slidingAnimation.Draw(spriteBatch, Position);
        }
    }

    private void CreateRunningAnimation(Texture2D spriteSheet)
    {
        for (int i = 0; i < NUMBER_OF_FRAME_IN_RUNNING_ANIMATION + 1; i++)
        {
            AddSpriteInFrameAnimation(spriteSheet, AVATAR_RUNNING_TEXTURE_POSX, AVATAR_RUNNING_TEXTURE_POSY, AVATAR_RUNNING_TEXTURE_WIDTH, AVATAR_RUNNING_TEXTURE_HEIGHT, i, NUMBER_OF_FRAME_IN_RUNNING_ANIMATION, _runningAnimation, RUNNING_ANIMATION_TIMESTAMP);
        }
    }

    private void AddSpriteInFrameAnimation(Texture2D spriteSheet, int defaultPosX, int defaultPosY, int defaultAvatarRunningWidth, int defaultAvatarRunningHeight, int positionOfFrame, int numberOfFrame, SpriteAnimation animation, float timeStamp)
    {
        if (positionOfFrame == numberOfFrame)
        {
            animation.AddFrame(_runningAnimation[0].Sprite, positionOfFrame * RUNNING_ANIMATION_TIMESTAMP);
        }
        else
        {
            animation.AddFrame(new Sprite(spriteSheet, defaultPosX + positionOfFrame * defaultAvatarRunningWidth, defaultPosY, defaultAvatarRunningWidth, defaultAvatarRunningHeight), positionOfFrame * timeStamp);
        }
    }

    private void CreateSlidingAnimation(Texture2D spriteSheet)
    {
        for (int i = 0; i < NUMBER_OF_FRAME_IN_SLIDING_ANNIMATION + 1; i++)
        {
            AddSpriteInFrameAnimation(spriteSheet, AVATAR_SLIDING_TEXTURE_POSX, AVATAR_RUNNING_TEXTURE_POSY, AVATAR_SLIDING_TEXTURE_WIDTH, AVATAR_SLIDING_TEXTURE_HEIGHT, i, NUMBER_OF_FRAME_IN_SLIDING_ANNIMATION, _slidingAnimation, SLIDING_ANIMATION_TIMESTAMP);
        }
    }
    
    protected virtual void OnJumpComplete()
    {
        EventHandler handler = JumpComplete;
        handler?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnDied()
    {
        EventHandler handler = Died;
        handler?.Invoke(this, EventArgs.Empty);
    }

    public void Initialize()
    {
        Speed = START_GAME_SPEED;
        State = AvatarState.Running;
        IsAlive = true;
    }

    public bool Die()
    {
        if (!IsAlive)
        {
            return false;
        }

        _hitSound.Play();
        IsAlive = false;
        Speed = 0;
        State = AvatarState.Idle;
        OnDied();
        return true;
    }
}