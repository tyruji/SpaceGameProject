using Godot;
using System;

public partial class RoboAnimator : Node3D
{
    [Export]
    public RoboModel Model { get; set; }

    public override void _Ready()
    {
        Model ??= GetParent().GetNode<RoboModel>( nameof( Model ) );
    }

    public override void _Process( double delta )
    {
        Model.Body.Position = .01f * MathHelper.FastRandomPointInUnitSphere();
        Model.Head.LookAt( GetNode<Node3D>( "../../Player" ).GlobalPosition, null, true );
    }
}
