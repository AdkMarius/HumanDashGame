using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HumanDash.Entities;

public abstract class Entity : IGameEntity
{
    protected Sprite Sprite { get; set; }
    
    public int DrawOrder { get; set; }
    
    public void Update(GameTime gameTime)
    {
        
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        
    }
}