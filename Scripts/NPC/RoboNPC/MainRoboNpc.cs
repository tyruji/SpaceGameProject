using Godot;
using System;
using static RoboStates;

public partial class MainRoboNpc : AnimatableBody3D, IStateHolder
{
    [Export]
    public Player Player { get; set; }

    [Export]
    public RoboAnimator RoboAnimator { get; set; }

    [Export]
    public WaypointHandler WaypointHandler { get; set; }

    public State State { get; set; } = ROBO_IDLE_STATE;

    public Vector3 WalkToPosition { get; set; }

    public override void _Ready()
    {
        RoboAnimator ??= GetNode<RoboAnimator>( nameof( RoboAnimator ) );
        WaypointHandler ??= GetNode<WaypointHandler>( nameof( WaypointHandler ) );
        Player ??= GetParent().GetNode<Player>( nameof( Player ) );
    }

    public override void _Process( double delta )
    {
        State?.Handle( this );
    }
}
