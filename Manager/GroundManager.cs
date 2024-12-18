using System;
using System.Collections.Generic;
using System.Linq;
using HumanDash.Entities;
using HumanDash.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HumanDash.Manager;

public class GroundManager : IGameEntity
{
    private const int TOTAL_TILES = 6;
    private const float GROUND_TILE_DEFAULT_POSY = 237;
    private const int TEXTURE_DEFAULT_WIDTH = 545;
    private const int TEXTURE_DEFAULT_HEIGHT = 90;

    private readonly List<GroundTile> _groundTiles;
    private readonly Random _random;
    
    private Avatar _avatar;
    public int DrawOrder { get; set; }

    public GroundManager(Texture2D groundTileTexture, Avatar avatar)
    {
        _groundTiles = new List<GroundTile>();
        _random = new Random();
        _avatar = avatar;

        CreateGroundTiles(groundTileTexture);
        Intialize();
    }

    private void CreateGroundTiles(Texture2D groundTileTexture)
    {
        for (int i = 0; i < TOTAL_TILES; i++)
        {
            var groundTile = new GroundTile(
                groundTileTexture,
                new Vector2(0, GROUND_TILE_DEFAULT_POSY),
                i,
                _avatar,
                TEXTURE_DEFAULT_WIDTH,
                TEXTURE_DEFAULT_HEIGHT
            );
            _groundTiles.Add(groundTile);
        }
    }

    private void Intialize()
    {
        // Shuffle the tiles randomly
        var shuffledTiles = _groundTiles.OrderBy(_ => _random.Next()).ToList();

        // Position the tiles sequentially, but use the shuffled order
        for (int i = 0; i < shuffledTiles.Count; i++)
        {
            shuffledTiles[i].PositionX = i * TEXTURE_DEFAULT_WIDTH;
        }

        // Update the ground tiles list with shuffled tiles
        _groundTiles.Clear();
        _groundTiles.AddRange(shuffledTiles);
    }

    private void SpawnTile(GroundTile groundTile)
    {
        // Randomly select a texture and reposition the tile to the right of the screen
        int randomIndex = _random.Next(0, _groundTiles.Count);
        var randomTile = _groundTiles[randomIndex];

        groundTile.Sprite = randomTile.Sprite;
        groundTile.PositionX = GetRightmostTilePosition() + TEXTURE_DEFAULT_WIDTH;
    }

    private float GetRightmostTilePosition()
    {
        // Get the PositionX of the tile farthest to the right
        return _groundTiles.Max(t => t.PositionX);
    }

    public void Update(GameTime gameTime)
    {
        foreach (var groundTile in _groundTiles)
        {
            groundTile.Update(gameTime);

            // If the tile moves off-screen, respawn it
            if (groundTile.PositionX + TEXTURE_DEFAULT_WIDTH < 0)
            {
                SpawnTile(groundTile);
            }
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        foreach (var tile in _groundTiles)
        {
            tile.Draw(gameTime, spriteBatch);
        }   
    }
}
