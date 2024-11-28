using System;
using System.Numerics;
using System.Runtime.InteropServices;
using HumanDash.Enum;

namespace HumanDash.Entities;

public class SpriteAnimationFrame : Entity
{
    private Sprite _sprite;

    public Sprite Sprite
    {
        get => _sprite;
        set
        {
           if (Sprite == null)
           {
               throw new ArgumentNullException(nameof(value), "Le sprite ne peut pas être null");

           }
           else
           {
               _sprite = value;
           }
        }
    }
    
    public double TimeStamp { get; }

    public SpriteAnimationFrame(Sprite sprite, double timeStamp)
    {
        Sprite = sprite;
        TimeStamp = timeStamp;
    }
}