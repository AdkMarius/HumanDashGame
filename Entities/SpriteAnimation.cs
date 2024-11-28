using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using HumanDash.Enum;
using Microsoft.Xna.Framework;

namespace HumanDash.Entities;

public class SpriteAnimation : Entity
{
    private List<SpriteAnimationFrame> _frames;
    public bool IsPlaying { get; private set; }

    public double _PlayBackProgess;
    
    public double PlayBackProgess
    {
        get => _PlayBackProgess;
        private set => _PlayBackProgess = value;
    }

    public void addFrame(Sprite sprite, double timeStamp)
    {
        _frames.Add(new SpriteAnimationFrame(sprite, timeStamp));
    }

    public void Update(GameTime gameTime)
    {
        if (IsPlaying)
        {
            _PlayBackProgess += gameTime.ElapsedGameTime.TotalSeconds;
        }
        else
        {
            Stop();
        }
    }
    
    public void Start()
    {
        IsPlaying = true;
        _PlayBackProgess = 0;
    }

    public void Stop()
    {
        IsPlaying = false;
        _PlayBackProgess = 0;
    }

    public SpriteAnimationFrame GetFrame(int index)
    {
        if (index < 0 || index >= _frames.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"Le frame avec l'index {index} n'existe pas.");
        }
        return _frames[index];
    }
    
    
}