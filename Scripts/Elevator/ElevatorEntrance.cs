using Godot;
using System;

public partial class ElevatorEntrance : Node3D
{
    [Export]
    public Elevator Elevator { get; set; }

    [Export]
    public Door ElevatorDoor { get; set; }

    [Export]
    public ElevatorCallButton ElevatorCallButton { get; set; }

    public void HandleElevatorStartMoving()
    {
        if( Elevator == null || ElevatorDoor == null || ElevatorCallButton == null ) return;

        ElevatorDoor.Close();
    }

    public void HandleElevatorReachDestination()
    {
        if( Elevator == null || ElevatorDoor == null || ElevatorCallButton == null ) return;

        float dist = Elevator.GlobalPosition.DistanceSquaredTo( ElevatorCallButton.ElevatorPosition );

            // Check if the elevator is on the same floor as the entrance.
        if( dist > .1f ) return;

            // If it is, open the entrance door.
        ElevatorDoor.Open();
    }
}
