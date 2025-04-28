using Godot;
using System;

public partial class CopyTransform : Node3D
{
    [Export]
    public Node3D NodeToCopy { get; set; } = null;

    [Export]
    public Vector3 Offset { get; set; } = new Vector3( 0, 0, -80 );

    public override void _Process( double delta )
    {
        if( NodeToCopy == null ) return;
        GlobalTransform = NodeToCopy.GlobalTransform;
        GlobalPosition += Offset;
    }
}
