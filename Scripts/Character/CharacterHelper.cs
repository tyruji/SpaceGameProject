using Godot;

public static class CharacterHelper
{
    public static void HandleBaseMovement( this ICharacter character, double delta )
    {
        float dt = ( float ) delta;

        var up = character.GlobalBasis.Y;

            // handle jumping.
        if( character.WantsToJump )
        {
                // Reset upward velocity to 0.
            character.Velocity -= up * character.Velocity.Dot( up );

            character.Velocity += character.GlobalBasis.Y * character.JumpSpeed;
            character.WantsToJump = false;
        }

            // Get the projected horizontal velocity based on upward axis.
        var upward_vel = up * character.Velocity.Dot( up );
        var horizontal_vel = character.Velocity - upward_vel;
        upward_vel -= dt * character.Gravity * up;
    
            // Add air acceleration and friction later on!
        var acc = character.MoveDirection.Dot( horizontal_vel ) > 0 ?
                                character.Acceleration : character.Friction;

        horizontal_vel = horizontal_vel.Lerp( character.MoveDirection * character.MaxSpeed, acc * dt );

        character.Velocity = horizontal_vel + upward_vel;
    }
}