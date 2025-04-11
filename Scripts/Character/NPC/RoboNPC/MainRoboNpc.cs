using Godot;
using System;
using static RoboStates;

public partial class MainRoboNpc : AnimatableBody3D, IStateHolder, ICharacter
{
    [Export]
    public Player Player { get; set; }

    [Export]
    public float Gravity { get; set; } = 24f;

    [Export]
    public float MaxSpeed { get; set; } = 20f;

    [Export]
    public float JumpSpeed { get; set; } = 18f;

    [Export]
    public float Acceleration { get; set; } = 4.5f;

    [Export]
    public float Friction { get; set; } = 16;

    [Export]
    public bool WantsToJump { get; set; } = false;

    [Export]
    public RoboAnimator RoboAnimator { get; set; }

    [Export]
    public WaypointHandler WaypointHandler { get; set; }

    public State State { get; set; } = ROBO_IDLE_STATE;

    public Vector3 MoveDirection { get; set; } = Vector3.Zero;

    public Vector3 Velocity { get; set; }

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

    public override void _PhysicsProcess( double delta )
    {
        this.HandleBaseMovement( delta );
        MoveAndCollide( ( float ) delta * Velocity );
    }
}
