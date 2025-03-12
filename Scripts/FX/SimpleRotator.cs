using Godot;
using System;

[Tool]
public partial class SimpleRotator : Node3D
{
    [Export]
    public Vector3 RotationAxis = Vector3.Right;

    [Export]
    public float RotationSpeed = 10f;

    public override void _Process( double delta )
    {
        RotationDegrees += ( float ) delta * RotationSpeed * RotationAxis;
    }
}
