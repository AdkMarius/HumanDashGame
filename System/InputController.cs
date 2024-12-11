using HumanDash.Entities;
using HumanDash.Enum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HumanDash.System;

public class InputController
{
    private Avatar _avatar;
    private KeyboardState _previousKeyboardState;

    public InputController(Avatar avatar)
    {
        _avatar = avatar;
    }

    public void ProcessControls(GameTime gameTime)
    {
        KeyboardState currentKeyboardState = Keyboard.GetState();
        bool isJumpingKeyPressed = currentKeyboardState.IsKeyDown(Keys.Up) || currentKeyboardState.IsKeyDown(Keys.Space);
        bool wasJumpingKeyPressed = _previousKeyboardState.IsKeyDown(Keys.Up) || _previousKeyboardState.IsKeyDown(Keys.Space);

        if (!wasJumpingKeyPressed && isJumpingKeyPressed)
        {
            if (_avatar.State != AvatarState.Jumping)
            {
                _avatar.StartJump();
            }
        } else if (!isJumpingKeyPressed && _avatar.State == AvatarState.Jumping)
        {
            _avatar.CancelJump();
        } else if (currentKeyboardState.IsKeyDown(Keys.Down))
        {
            if (_avatar.State == AvatarState.Jumping || _avatar.State == AvatarState.Falling)
            {
                _avatar.Drop();
            }
            else
            {
                _avatar.Slide();
            }
        } else if (_avatar.State == AvatarState.Sliding && !currentKeyboardState.IsKeyDown(Keys.Down))
        {
            _avatar.GetUp();
        }
        
        _previousKeyboardState = currentKeyboardState;
    }
}