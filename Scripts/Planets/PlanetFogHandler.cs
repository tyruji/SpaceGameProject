using Godot;
using System;

public partial class PlanetFogHandler : MeshInstance3D
{
    [Export]
    public PlanetEnterHandler PlanetEnterHandler { get; set; }

    [Export]
    public float FogResetTime { get; set; } = 5f;

    private float _timeElapsed = 0f;

    private float _startTransparency = .0f;

    public override void _Ready()
    {
        PlanetEnterHandler ??= GetParent<PlanetEnterHandler>();
        PlanetTransitionStates.PLANET_LOADING_STATE.OnEnterPlanet += ( enterHandler ) =>
        {
            _timeElapsed = 0f;
            _startTransparency = Transparency;
        };
    }

    public override void _Process( double delta )
    {
        _timeElapsed += ( float ) delta;
        _timeElapsed = Mathf.Min( _timeElapsed, FogResetTime );
        Transparency = Mathf.Lerp( _startTransparency, CalculateTransparency(), _timeElapsed / FogResetTime );

        if( Transparency < 1.0f ) Visible = true;
        else Visible = false;
    }

    public float CalculateTransparency()
    {
        var planet = PlanetEnterHandler.ClosestPlanet;
        
        if( planet == null ) return 1f;

        var cam_pos = PlanetEnterHandler.CameraHandler.GlobalPosition;
        GlobalPosition = cam_pos;

        float planet_dist_sqr = planet.DistanceSqrToPoint( cam_pos );

        float start_dist_sqr = planet.StartFogDistance * planet.StartFogDistance;
        float end_dist_sqr = planet.EndFogDistance * planet.EndFogDistance;

        if( planet_dist_sqr > start_dist_sqr ) return 1f;

        float alpha = ( planet_dist_sqr - start_dist_sqr ) / ( end_dist_sqr - start_dist_sqr );
        return 1.0f - alpha;
    }
}
