using Godot;
using System;

public partial class Planet : Node3D, IPlanet
{
    [Export]
    public string WorldScenePath { get; set; } = string.Empty;

    [Export]
    public float StartFogDistance { get; set; } = 1000;

    [Export]
    public float EndFogDistance { get; set; } = 700;

    [Export]
    public float PlanetEnterDistance { get; set; } = 550;

    public float DistanceSqrToPoint( Vector3 point ) => ( GlobalPosition - point ).LengthSquared();
}
