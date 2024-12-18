using System;
using HumanDash.Enum;
using HumanDash.Graphics;
using HumanDash.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HumanDash.Entities;

public class GroundObstacle : Obstacle
{
    private const int TRASH_TEXTURE_POS_X = 330;
    private const int TRASH_TEXTURE_POS_Y = 420;
    private const int TRASH_TEXTURE_WIDTH = 50;
    private const int TRASH_TEXTURE_HEIGHT = 70;
    
    private const int STOP_TEXTURE_POS_X = 160;
    private const int STOP_TEXTURE_POS_Y = 20;
    private const int STOP_TEXTURE_WIDTH = 90;
    private const int STOP_TEXTURE_HEIGHT = 110;
    
    private const int PANEL_TEXTURE_POS_X = 90;
    private const int PANEL_TEXTURE_POS_Y = 412;
    private const int PANEL_TEXTURE_WIDTH = 38;
    private const int PANEL_TEXTURE_HEIGHT = 78;
    
    private static int _spriteWidth
    {
        get
        {
            Random random = new Random();
            int randomValue = random.Next((int)TrashSize.Small, (int)TrashSize.Large + 1);
            return TRASH_TEXTURE_WIDTH + randomValue * TRASH_TEXTURE_WIDTH;
        }
    }

    public override Rectangle CollisionBox
    {
        get
        {
            Rectangle box = new Rectangle(
                (int)Math.Round(Position.X),
                (int)Math.Round(Position.Y),
                Sprite.Width,
                Sprite.Height
            );
            box.Inflate(-20, -5);
            return box;
        }
    }
    
    public GroundObstacle(Texture2D texture, Avatar avatar, Vector2 position, ObstacleType obstacleType) : base(avatar, position)
    {
        int posX = 0;
        int posY = 0;
        int width = 0;
        int height = 0;
        
        switch (obstacleType)
        {
            case ObstacleType.Panel:
                posX = PANEL_TEXTURE_POS_X;
                posY = PANEL_TEXTURE_POS_Y;
                width = PANEL_TEXTURE_WIDTH;
                height = PANEL_TEXTURE_HEIGHT;
                break;
            
            case ObstacleType.StopPanel:
                posX = STOP_TEXTURE_POS_X;
                posY = STOP_TEXTURE_POS_Y;
                width = STOP_TEXTURE_WIDTH;
                height = STOP_TEXTURE_HEIGHT;
                break;
            
            case ObstacleType.Trash:
                posX = TRASH_TEXTURE_POS_X;
                posY = TRASH_TEXTURE_POS_Y;
                width = _spriteWidth;
                height = TRASH_TEXTURE_HEIGHT;
                break;
        }
        
        Sprite = new Sprite(texture, posX, posY, width, height);
    }


    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        Sprite.Draw(spriteBatch, Position);
    }
}