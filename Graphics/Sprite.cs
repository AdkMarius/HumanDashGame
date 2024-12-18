using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HumanDash.Graphics;

public class Sprite
{
    private int X { get; }
    private int Y { get; }
    public int Width { get; }
    public int Height { get; }
    private Texture2D Texture { get; }

    private Color TintColor { get; set; } = Color.White;

    public Sprite(Texture2D texture, int x, int y, int width, int height)
    {
        Texture = texture;
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        spriteBatch.Draw(Texture, position, new Rectangle(X, Y, Width, Height), TintColor);
    }
}