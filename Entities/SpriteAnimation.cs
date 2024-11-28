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

    public double _PlayBackProgess;//temps depuis le début de l'animation en cours
    
    public double PlayBackProgess
    {
        get => _PlayBackProgess;
        private set => _PlayBackProgess = value;
    }

    public void addFrame(Sprite sprite, double timeStamp)//ajoute un frame à notre liste de frames en attribut
    {
        _frames.Add(new SpriteAnimationFrame(sprite, timeStamp));
    }

    public void Update(GameTime gameTime)
    {
        if (IsPlaying)
        {
            _PlayBackProgess += gameTime.ElapsedGameTime.TotalSeconds;//Met à jour le playback progress définit plus haut
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

    public void Stop()//Arrete l'animation
    {
        IsPlaying = false;
        _PlayBackProgess = 0;
    }

    public SpriteAnimationFrame GetFrame(int index)//renvoie le frame à l'indice de la liste correspondant
    {
        if (index < 0 || index >= _frames.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"Le frame avec l'index {index} n'existe pas.");
        }
        return _frames[index];
    }
    
    
}