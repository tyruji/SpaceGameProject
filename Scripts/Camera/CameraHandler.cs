using Godot;
using System;

public partial class CameraHandler : Camera3D
{
    public ICameraMode CameraMode { get; set; }

    public override void _Process( double delta )
    {
        CameraMode?.Handle( this, delta );
    }
}
