using Godot;
using System;

public partial class PlanetEnterHandler : Node3D
{
    [Export]
    public CameraHandler CameraHandler { get; set; }

    [Export]
    public PlanetContainer PlanetContainer { get; set; }

    public Planet ClosestPlanet { get; set; }

    public override void _Ready()
    {
        CameraHandler ??= GetParent().GetNode<CameraHandler>( nameof( CameraHandler ) );
        PlanetContainer ??= GetParent().GetNode<PlanetContainer>( nameof( PlanetContainer ) );
    }

        // Called regularly after a period of time, preferably like 2seconds or less or more, test it out.
    public void UpdateClosestPlanet()
    {
        float dist_sqr = float.MaxValue;
        foreach( var planet in PlanetContainer.Planets )
        {
            float new_dist_sqr = ( CameraHandler.GlobalPosition - planet.GlobalPosition ).LengthSquared();

            if( new_dist_sqr >= dist_sqr ) continue;
            dist_sqr = new_dist_sqr;
            ClosestPlanet = planet;
        }
    }
}
