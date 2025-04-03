using System;
using Godot;

public static class PlanetTransitionStates
{
    public static readonly PlanetLoadingTransitionState PLANET_LOADING_STATE = new PlanetLoadingTransitionState();
    public static readonly PlanetAwaitTransitionState PLANET_AWAIT_TRANSITION_STATE = new PlanetAwaitTransitionState();
}

public abstract class PlanetTransitionState : State {}

public class PlanetAwaitTransitionState : PlanetTransitionState
{
    public override void Handle( IStateHolder stateHolder, object arg = null )
    {
        PlanetEnterHandler enterHandler = stateHolder as PlanetEnterHandler;
        var closest_planet = enterHandler.ClosestPlanet;
        var current_planet = enterHandler.CurrentPlanet;

        if( closest_planet == null || current_planet == null ) return;
        if( closest_planet == current_planet ) return;

        var cam_pos = enterHandler.CameraHandler.GlobalPosition;
        float planet_dist_sqr = closest_planet.DistanceSqrToPoint( cam_pos );

        float enter_planet_dist_sqr = closest_planet.PlanetEnterDistance * closest_planet.PlanetEnterDistance;

        if( planet_dist_sqr > enter_planet_dist_sqr ) return;

        ChangeState( stateHolder, PlanetTransitionStates.PLANET_LOADING_STATE );

            // Hide the planet mesh.
        ( closest_planet as Node3D )?.Hide();
    }
}

public class PlanetLoadingTransitionState : PlanetTransitionState
{
    public event Action<PlanetEnterHandler> OnEnterPlanet;

    protected static readonly Godot.Collections.Array LOAD_PROGRESS_ARR = [];

    public const string LEVEL_NODE_NAME = "Level"; 

    public override void Enter( IStateHolder stateHolder )
    {
        PlanetEnterHandler enterHandler = stateHolder as PlanetEnterHandler;
        var planet = enterHandler.ClosestPlanet;

        ResourceLoader.LoadThreadedRequest( planet.WorldScenePath );
    }
    public override void Handle( IStateHolder stateHolder, object arg = null )
    {
        PlanetEnterHandler enterHandler = stateHolder as PlanetEnterHandler;
        var planet = enterHandler.ClosestPlanet;

        ResourceLoader.LoadThreadedGetStatus( planet.WorldScenePath, LOAD_PROGRESS_ARR );
        var progress = ( float ) LOAD_PROGRESS_ARR[ 0 ];

            // The progress is between 0. and 1., so we should start loading at approx 1.
        if( progress < .99f ) return;
    
            // Load the world scene.
        var world_scene = ( PackedScene ) ResourceLoader.LoadThreadedGet( planet.WorldScenePath );
        var level_node = enterHandler.GetParent().GetNode( LEVEL_NODE_NAME );

            // Remove current level.
        foreach( var child in level_node.GetChildren() ) child.QueueFree(); 
    
        var world_instance = world_scene.Instantiate();
        level_node.AddChild( world_instance );

        enterHandler.CurrentPlanet = planet;

        OnEnterPlanet?.Invoke( enterHandler );
        ChangeState( stateHolder, PlanetTransitionStates.PLANET_AWAIT_TRANSITION_STATE );
    }
}