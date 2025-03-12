using Godot;
using System;

public partial class PlayerStepStompInvoker : Node
{
    [Export]
    public Player Player { get; set; }

    private bool _playerMidair = false;

    public override void _Ready()
    {
        Player ??= GetParent<Player>();
    }

    public override void _Process( double delta )
    {
        if( !Player.EnableControl ) return;

        if( !Player.IsOnFloor() )
        {
            _playerMidair = true;
            return;
        }
        
        for( int i = 0; i < Player.GetSlideCollisionCount(); ++i )
        {
            var col = Player.GetSlideCollision( i ).GetCollider();
            
            if( Player.inputDirection.LengthSquared() > 0 && col is ISteppable steppable )
            {
                steppable.StepOn();
            }

            if( !_playerMidair || col is not IStompable stompable ) continue;

            stompable.StompOn();
        }
    
        _playerMidair = false;
    }
}
