using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HumanDash.Entities;
using HumanDash.Enum;
using HumanDash.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HumanDash.Manager;

public class ObstacleManager : IGameEntity
{
    private const int MIN_OBSTACLE_DISTANCE = 20;
    private const int MAX_OBSTACLE_DISTANCE = 50;
    private const float MIN_SPAWN_DISTANCE = 20f;
    
    private const int OBSTACLE_DISTANCE_SPEED_TOLERANCE = 5;
    private const int OBSTACLE_DRAW_ORDER = 12;
    private const int OBSTACLE_SPRITE_POS_Y = 180;

    private ScoreBoard ScoreBoard { get; }

    private double LastSpawnScore { get; set; }
    private double CurrentTargetDistance { get; set; }

    public bool IsEnabled { get; set; }
    
    private Random _random;
    private Avatar _avatar;

    private Texture2D _spriteSheet;

    // list of obstacles
    private readonly List<Obstacle> _obstacles;
    
    // a list of removed obstacles
    private readonly List<Obstacle> _removedObstacles;
    
    private bool CanSpawnObstacles => IsEnabled && ScoreBoard.Score >= MIN_SPAWN_DISTANCE;

    public ObstacleManager(Avatar avatar, Texture2D spriteSheet, ScoreBoard scoreBoard)
    {
        _random = new Random();
        _avatar = avatar;
        _spriteSheet = spriteSheet;
        ScoreBoard = scoreBoard;
        _obstacles = new List<Obstacle>();
        _removedObstacles = new List<Obstacle>();
    }
    
    public int DrawOrder { get; set; }

    private void SpawnRandomObstacle()
    {
        ObstacleType obstacleType = (ObstacleType) _random.Next((int)ObstacleType.Panel, (int)ObstacleType.Trash + 1);
        int posY = 0;

        switch (obstacleType)
        {
            case ObstacleType.Panel:
                posY = OBSTACLE_SPRITE_POS_Y;
                break;
            case ObstacleType.Trash:
                posY = OBSTACLE_SPRITE_POS_Y + 8;
                break;
            case ObstacleType.StopPanel:
                posY = OBSTACLE_SPRITE_POS_Y - 15;
                break;
        }

        Vector2 position = new Vector2(HumanDashGame.WINDOW_WIDTH, posY);
        var obstacle = new GroundObstacle(_spriteSheet, _avatar, position, obstacleType)
        {
            DrawOrder = OBSTACLE_DRAW_ORDER
        };

        _obstacles.Add(obstacle);
    }

    public void Update(GameTime gameTime)
    {
        if (!IsEnabled)
            return;

        if (CanSpawnObstacles && (LastSpawnScore <= 0 || ScoreBoard.Score - LastSpawnScore >= CurrentTargetDistance))
        {
            CurrentTargetDistance = _random.NextDouble() * (MAX_OBSTACLE_DISTANCE - MIN_OBSTACLE_DISTANCE) +
                                     MIN_OBSTACLE_DISTANCE;

            CurrentTargetDistance += (_avatar.Speed - Avatar.START_GAME_SPEED) / (Avatar.MAX_SPEED - Avatar.START_GAME_SPEED) * OBSTACLE_DISTANCE_SPEED_TOLERANCE;
            
            LastSpawnScore = ScoreBoard.Score;

            SpawnRandomObstacle();
        }

        for (int i = _obstacles.Count - 1; i >= 0; i--)
        {
            var obstacle = _obstacles[i];
            obstacle.Update(gameTime);

            if (obstacle.Position.X + obstacle.Sprite.Width < 0)
            {
                _obstacles.RemoveAt(i);
            }
        }

        foreach (Obstacle obs in _removedObstacles)
        {
            _obstacles.Remove(obs);
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        foreach (var obstacle in _obstacles)
        {
            obstacle.Draw(gameTime, spriteBatch);
        }
    }

    public void RemoveAllObstacles()
    {
        foreach (Obstacle obs in _obstacles)
        {
            _removedObstacles.Add(obs);
        }
    }

    public void Initialize()
    {
        LastSpawnScore = 0;
        CurrentTargetDistance = 0;
        IsEnabled = true;
    }
}
