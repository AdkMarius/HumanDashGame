using Microsoft.Xna.Framework;

namespace HumanDash.Interfaces;

public interface ICollidable
{ 
    Rectangle CollisionBox { get; }
}