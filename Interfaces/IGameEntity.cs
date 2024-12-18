using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HumanDash.Interfaces;

public interface IGameEntity
{
    int DrawOrder { get; set; }
    
    void Update(GameTime gameTime);
    
    void Draw(GameTime gameTime, SpriteBatch spriteBatch);
}