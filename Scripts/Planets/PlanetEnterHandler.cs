using Godot;
using System;

public partial class PlanetEnterHandler : Node3D, IStateHolder
{
    [Export]
    public CameraHandler CameraHandler { get; set; }

    [Export]
    public PlanetContainer PlanetContainer { get; set; }

    /// <summary>
    /// This is the closest planet thats usually different than the CurrentPlanet.
    /// CurrentPlanet may be SpacePlanet... maybe change the name? because space is not a planet...
    /// </summary>
    public IPlanet ClosestPlanet { get; set; }

    /// <summary>
    /// The planet the player is currently on.
    /// </summary>
    public IPlanet CurrentPlanet { get; set; }

    public State State { get; set; }

    public override void _Ready()
    {
        CameraHandler ??= GetParent().GetNode<CameraHandler>( nameof( CameraHandler ) );
        PlanetContainer ??= GetParent().GetNode<PlanetContainer>( nameof( PlanetContainer ) );
    }

    public override void _Process( double delta )
    {
        State?.Handle( this );
    }

        // Called regularly after a period of time, preferably like 2seconds or less or more, test it out.
    public void UpdateClosestPlanet()
    {
        float dist_sqr = float.MaxValue;
        foreach( var planet in PlanetContainer.Planets )
        {
            if( planet == CurrentPlanet ) continue;

            float new_dist_sqr = ( CameraHandler.GlobalPosition - planet.GlobalPosition ).LengthSquared();

            if( new_dist_sqr >= dist_sqr ) continue;
            dist_sqr = new_dist_sqr;
            ClosestPlanet = planet;
        }
    }
}
