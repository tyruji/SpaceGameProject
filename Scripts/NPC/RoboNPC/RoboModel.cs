using Godot;
using System;

public partial class RoboModel : Node3D
{
    [Export]
    public MeshInstance3D Head { get; set; }

    [Export]
    public MeshInstance3D Body { get; set; }

    [Export]
    public MeshInstance3D EyeLeft { get; set; }

    [Export]
    public MeshInstance3D EyeRight { get; set; }

    public override void _Ready()
    {
        Head ??= GetNode<MeshInstance3D>( nameof( Head ) );
        Body ??= GetNode<MeshInstance3D>( nameof( Body ) );
        EyeLeft ??= Head.GetNode<MeshInstance3D>( nameof( EyeLeft ) );
        EyeRight??= Head.GetNode<MeshInstance3D>( nameof( EyeRight) );
    }
}
