using Godot;

public partial class SpacePlanet : Node3D, IPlanet
{
    [Export]
    public string WorldScenePath { get; set; } = string.Empty;

    [Export]
    public float StartFogDistance { get; set; } = 1000;

    [Export]
    public float EndFogDistance { get; set; } = 700;

    [Export]
    public float PlanetEnterDistance { get; set; } = 550;

    [Export]
    public float EnterSpaceHeight { get; set; } = 500f;

    public float DistanceSqrToPoint( Vector3 point )
    {
        float dist = point.Y - EnterSpaceHeight;
        return dist * dist;
    }
}