using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using HumanDash.Entities;
using HumanDash.Enum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = System.Numerics.Vector2;

namespace HumanDash.Graphics;

public class SpriteAnimation
{
    private List<SpriteAnimationFrame> _frames;
    
    public SpriteAnimationFrame this[int index] => GetFrame(index);
    
    public bool IsPlaying { get; private set; }

    // temps depuis le début de l'animation en cours
    public double PlayBackProgess { get; private set; }
    
    // renvoyer le frame actuel qui doit etre dessiné à l'écran
    public SpriteAnimationFrame CurrentFrame
    {
        get
        {
            return _frames
                .Where(f => f.TimeStamp <= PlayBackProgess)
                .OrderBy(f => f.TimeStamp)
                .LastOrDefault();
        }
    }
    
    // renvoyer le TimeStamp maximal de l'animation
    public double Duration
    {
        get
        {
            if (_frames.Count == 0)
                return 0;
            
            return _frames.Max(f => f.TimeStamp);
        }
    }

    public SpriteAnimation()
    {
        _frames = new List<SpriteAnimationFrame>();
    }

    // ajoute un frame à notre liste de frames en attribut
    public void AddFrame(Sprite sprite, double timeStamp)
    {
        _frames.Add(new SpriteAnimationFrame(sprite, timeStamp));
    }

    public void Update(GameTime gameTime)
    {
        // Met à jour le playback progress définit plus haut
        PlayBackProgess += gameTime.ElapsedGameTime.TotalSeconds;
        
        if (IsPlaying)
        {
            if (PlayBackProgess >= Duration)
            {
                PlayBackProgess %= Duration;
            }
        }
        else
        {
            Stop();
        }
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        SpriteAnimationFrame frame = CurrentFrame;
        frame?.Sprite.Draw(spriteBatch, position);
    }
    
    public void Start()
    {
        IsPlaying = true;
    }

    // Arrete l'animation
    public void Stop()
    {
        IsPlaying = false;
        PlayBackProgess = 0;
    }

    // renvoie le frame à l'indice de la liste correspondant
    public SpriteAnimationFrame GetFrame(int index)
    {
        if (index < 0 || index >= _frames.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"Le frame avec l'index {index} n'existe pas.");
        }
        return _frames[index];
    }
    
    
}