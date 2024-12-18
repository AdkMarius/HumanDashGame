using HumanDash.Entities;

namespace HumanDash.Manager;

public class CollisionManager
{
    private Avatar _avatar;
    
    private Obstacle _obstacle;

    public CollisionManager(Avatar avatar, Obstacle obstacle)
    {
        _avatar = avatar;
        _obstacle = obstacle;
    }

    public void CheckCollision()
    {
        if (_avatar.CollisionBox.Intersects(_obstacle.CollisionBox))
        {
            _avatar.Die();
        }
    }
}