using System;
using HumanDash.Graphics;
using HumanDash.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HumanDash.Entities;

public class GameOverScreen : IGameEntity
{
    private const int GAME_OVER_TEXTURE_POS_X = 655;
    private const int GAME_OVER_TEXTURE_POS_Y = 14;
    public const int GAME_OVER_SPRITE_WIDTH = 192;
    private const int GAME_OVER_SPRITE_HEIGHT = 14;
    
    private const int BUTTON_TEXTURE_POS_X = 1;
    private const int BUTTON_TEXTURE_POS_Y = 1;
    
    private const int BUTTON_SPRITE_WIDTH = 38;
    private const int BUTTON_SPRITE_HEIGHT = 34;

    private Sprite _textSprite;
    private Sprite _buttonSprite;
    
    private Rectangle ButtonBounds => 
        new Rectangle(
            ButtonPosition.ToPoint(), 
            new Point(BUTTON_SPRITE_WIDTH, BUTTON_SPRITE_HEIGHT));

    public bool IsEnabled { get; set; }
    
    public Vector2 Position { get; set; }

    private Vector2 ButtonPosition => Position + new Vector2(GAME_OVER_SPRITE_WIDTH / 2 - BUTTON_SPRITE_WIDTH / 2,
        GAME_OVER_SPRITE_HEIGHT + 20);

    public int DrawOrder { get; set; } = 20;
    
    public event EventHandler Replay;

    public GameOverScreen(Texture2D texture)
    {
        _textSprite = new Sprite(texture, GAME_OVER_TEXTURE_POS_X, GAME_OVER_TEXTURE_POS_Y, GAME_OVER_SPRITE_WIDTH, GAME_OVER_SPRITE_HEIGHT);
        _buttonSprite = new Sprite(texture, BUTTON_TEXTURE_POS_X, BUTTON_TEXTURE_POS_Y, BUTTON_SPRITE_WIDTH, BUTTON_SPRITE_HEIGHT);
    }

    public void Update(GameTime gameTime)
    {
        if (!IsEnabled)
        {
            return;
        }
        
        MouseState mouseState = Mouse.GetState();
        
        if (ButtonBounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
        {
            OnReplay();
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (!IsEnabled)
            return;
        
        _textSprite.Draw(spriteBatch, Position);
        _buttonSprite.Draw(spriteBatch, ButtonPosition);
    }

    protected virtual void OnReplay()
    {
        EventHandler handler = Replay;
        handler?.Invoke(this, EventArgs.Empty);
    }
}