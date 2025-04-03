using Godot;

public class SpacePlanet : IPlanet
{
    public string WorldScenePath { get; set; } = @"";

    public float StartFogDistance { get; set; } = 1000;

    public float EndFogDistance { get; set; } = 700;

    public float PlanetEnterDistance { get; set; } = 550;

    public float DistanceSqrToPoint( Vector3 point )
    {
            // TODO:
            // Judge the distance only on the vertical component of the vector!
        float dist = (Vector3.Zero - point ).LengthSquared();
        return dist;
    }
}