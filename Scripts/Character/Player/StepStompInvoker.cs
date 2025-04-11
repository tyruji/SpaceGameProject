using Godot;
using System;

public partial class StepStompInvoker : Node
{
    [Export]
    public CharacterBody3D Character { get; set; }

    private bool _characterMidair = false;

    public override void _Ready()
    {
        Character ??= GetParent<Player>();
    }

    public override void _Process( double delta )
    {
        if( Character is IControllable ctrlble && !ctrlble.EnableControl ) return;

        if( !Character.IsOnFloor() )
        {
            _characterMidair = true;
            return;
        }
        
        for( int i = 0; i < Character.GetSlideCollisionCount(); ++i )
        {
            var col = Character.GetSlideCollision( i ).GetCollider();
            
                // .1f is a small number, so there is no stomp when
                // the character is moving slowly, or slowing down. 
            if( Character.Velocity.LengthSquared() > 0.1f && col is ISteppable steppable )
            {
                steppable.StepOn();
            }

            if( !_characterMidair || col is not IStompable stompable ) continue;

            stompable.StompOn();
        }
    
        _characterMidair = false;
    }
}
