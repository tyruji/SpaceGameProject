using Godot;
using System;

public partial class PlanetFogHandler : MeshInstance3D
{
    [Export]
    public PlanetEnterHandler PlanetEnterHandler { get; set; }

    public override void _Ready()
    {
        PlanetEnterHandler ??= GetParent<PlanetEnterHandler>();
    }


    public override void _Process( double delta )
    {
        var planet = PlanetEnterHandler.ClosestPlanet;
        
        if( planet == null ) 
        {
            Visible = false;
            return;
        }

        var cam_pos = PlanetEnterHandler.CameraHandler.GlobalPosition;
        GlobalPosition = cam_pos;

        float planet_dist_sqr = ( cam_pos - planet.GlobalPosition ).LengthSquared();

        float start_dist_sqr = planet.StartFogDistance * planet.StartFogDistance;
        float end_dist_sqr = planet.EndFogDistance * planet.EndFogDistance;

        if( planet_dist_sqr > start_dist_sqr )
        {
            Visible = false;
            return;
        }
        Visible = true;

        float alpha = ( planet_dist_sqr - start_dist_sqr ) / ( end_dist_sqr - start_dist_sqr );
        Transparency = 1.0f - alpha;

    }
}
