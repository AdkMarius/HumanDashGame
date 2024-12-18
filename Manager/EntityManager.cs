using System;
using System.Collections.Generic;
using System.Linq;
using HumanDash.Entities;
using HumanDash.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HumanDash.Manager;

public class EntityManager
{
    private readonly List<IGameEntity> _entities;

    private readonly List<IGameEntity> _entitiesToAdd;
    
    private readonly List<IGameEntity> _entitiesToRemove;

    public EntityManager()
    {
        _entities = new List<IGameEntity>();
        _entitiesToAdd = new List<IGameEntity>();
        _entitiesToRemove = new List<IGameEntity>();
    }

    public void AddEntity(IGameEntity entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity), "Entity cannot be null");

        _entitiesToAdd.Add(entity);
    }

    public void RemoveEntity(IGameEntity entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
        
        _entitiesToRemove.Add(entity);
    }

    public void UpdateEntities(GameTime gameTime)
    {
        foreach (IGameEntity entity in _entities)
        {
            entity.Update(gameTime);
        }

        foreach (var entity in _entitiesToAdd)
        {
            _entities.Add(entity);
        }

        foreach (var entity in _entitiesToRemove)
        {
            _entities.Remove(entity);
        }
        
        _entitiesToAdd.Clear();
        _entitiesToRemove.Clear();
    }

    public void DrawEntities(GameTime gameTime, SpriteBatch spriteBatch)
    {
        foreach (var entity in _entities.OrderBy(f => f.DrawOrder))
        {
            entity.Draw(gameTime, spriteBatch);
        }
    }

    public void ClearEntities()
    {
        _entitiesToRemove.AddRange(_entities);
    }
}