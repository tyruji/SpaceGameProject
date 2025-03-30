using Godot;
using System;

public partial class Planet : Node3D
{
    [Export]
    public float StartFogDistance { get; set; } = 1000;

    [Export]
    public float EndFogDistance { get; set; } = 700;

        
}
