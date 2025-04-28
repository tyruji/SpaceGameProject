using Godot;
using System;

public partial class Elevator : AnimatableBody3D
{
    [Signal]
    public delegate void ElevatorReachDestinationEventHandler();

    [Signal]
    public delegate void ElevatorStartMovingEventHandler();

    [Export]
    public float Speed { get; set; } = 5f;

    [Export]
    public float StartMovingDelay { get; set; } = 1.5f;

    [Export]
    public Door ElevatorDoor { get; set; }

    public bool Busy { get; private set; }

    private Vector3 _targetPosition = Vector3.Zero;

    public override void _Ready()
    {
        ElevatorDoor ??= GetNode<Door>( nameof( ElevatorDoor ) );
        _targetPosition = GlobalPosition;
    }

    public void CallElevator( Vector3 position )
    {
        float dist = GlobalPosition.DistanceTo( position );

        if( Busy ) return;

            // If already at the target position.
        if( dist < .1f )
        {
                // Open the door, when the elevator is at the target position, but closed.
            if( position == _targetPosition && ElevatorDoor.Closed ) ElevatorDoor.Open();
            return;
        }

        Busy = true;
        _targetPosition = position;

        var tween = CreateTween();

        ElevatorDoor.Close();
        tween.TweenInterval( StartMovingDelay );

        EmitSignal( SignalName.ElevatorStartMoving );

        tween.TweenProperty( this, "global_position", position, dist / Speed )
                                .SetTrans( Tween.TransitionType.Back )
                                .SetEase( Tween.EaseType.Out );
        tween.TweenCallback( Callable.From( ReachDestination ) );
    }

    private void ReachDestination()
    {
        EmitSignal( SignalName.ElevatorReachDestination );
        Busy = false;
        ElevatorDoor.Open();
    }
}
