using System;
using HumanDash.Graphics;
using HumanDash.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HumanDash.Entities;

public class GroundTile : IGameEntity
{
    private const int TEXTURE_DEFAULT_POS_X = 0;
    private const int TEXTURE_DEFAULT_POS_Y = 9;
    
    public Sprite Sprite { get; set; }
    public float PositionX { get; set; }
    private float _positionY { get; }
    
    public int DrawOrder { get; set; }
    
    private Avatar _avatar;

    public GroundTile(Texture2D spriteTexture2D, Vector2 position, int spriteNumber, Avatar avatar, int defaultTextureWidth, int defaultTextureHeight)
    {
        Sprite = new Sprite(spriteTexture2D, TEXTURE_DEFAULT_POS_X, TEXTURE_DEFAULT_POS_Y + spriteNumber * defaultTextureHeight,
            defaultTextureWidth, defaultTextureHeight);
        PositionX = position.X;
        _positionY = position.Y;
        _avatar = avatar;
    }

    public void Update(GameTime gameTime)
    {
        PositionX -= _avatar.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        Sprite.Draw(spriteBatch, new Vector2(PositionX, _positionY));
    }
}