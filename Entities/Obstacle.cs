using HumanDash.Graphics;
using HumanDash.Interfaces;
using HumanDash.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HumanDash.Entities;

public abstract class Obstacle : IGameEntity, ICollidable
{
    public int DrawOrder { get; set; }
    
    public abstract Rectangle CollisionBox { get; }
    
    public Vector2 Position { get; private set; }

    private Avatar _avatar;
    
    public Sprite Sprite { get; set; }
    
    private CollisionManager _collisionManager;

    protected Obstacle(Avatar avatar, Vector2 position)
    {
        _avatar = avatar;
        Position = position;
        _collisionManager = new CollisionManager(_avatar, this);
    }

    public void Update(GameTime gameTime)
    {
        float posX = Position.X - _avatar.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        Position = new Vector2(posX, Position.Y);
        
        _collisionManager.CheckCollision();
    }

    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
}