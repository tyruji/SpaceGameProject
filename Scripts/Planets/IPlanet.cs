using Godot;

public interface IPlanet
{
    public string WorldScenePath { get; set; }

    public float StartFogDistance { get; set; }

    public float EndFogDistance { get; set; }

    public float PlanetEnterDistance { get; set; }

    float DistanceSqrToPoint( Vector3 point );
}