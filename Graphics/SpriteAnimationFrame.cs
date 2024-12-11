using System;
using System.Numerics;
using System.Runtime.InteropServices;
using HumanDash.Entities;
using HumanDash.Enum;

namespace HumanDash.Graphics;

public class SpriteAnimationFrame
{
    private Sprite _sprite;

    public Sprite Sprite
    {
        get => _sprite;
        set => _sprite = value ?? throw new ArgumentNullException(nameof(value), "Sprite cannot be null");
    }
    
    public double TimeStamp { get; }

    public SpriteAnimationFrame(Sprite sprite, double timeStamp)
    {
        Sprite = sprite;
        TimeStamp = timeStamp;
    }
}