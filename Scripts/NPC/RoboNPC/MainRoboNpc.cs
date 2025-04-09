using Godot;
using System;
using static RoboStates;

public partial class MainRoboNpc : AnimatableBody3D, IStateHolder
{
    [Export]
    public Player Player { get; set; }

    [Export]
    public RoboAnimator RoboAnimator { get; set; }

    public State State { get; set; } = ROBO_IDLE_STATE;

    public override void _Ready()
    {
        RoboAnimator ??= GetNode<RoboAnimator>( nameof( RoboAnimator ) );
        Player ??= GetParent().GetNode<Player>( nameof( Player ) );
    }

    public override void _Process( double delta )
    {
        State?.Handle( this );
    }
}
