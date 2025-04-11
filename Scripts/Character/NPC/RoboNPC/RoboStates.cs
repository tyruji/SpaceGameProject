using Godot;
using static RoboStates;

public static class RoboStates
{
    public static readonly RoboIdleState ROBO_IDLE_STATE = new RoboIdleState();
    public static readonly RoboWalkingState ROBO_WALKING_STATE = new RoboWalkingState();
}

public abstract class RoboState : State {}

public class RoboIdleState : RoboState
{
    public override void Handle( IStateHolder stateHolder, object arg = null )
    {
        if( stateHolder is not MainRoboNpc robo ) return;

        robo.RoboAnimator.HeadLookAtPosition = robo.Player.GlobalPosition;
        robo.MoveDirection = Vector3.Zero;

        if( robo.WaypointHandler.waypointType == WaypointHandler.eWaypointType.NONE ) return;
        ChangeState( stateHolder, ROBO_WALKING_STATE );
    }
}

public class RoboWalkingState : RoboState
{
    public override void Handle( IStateHolder stateHolder, object arg = null )
    {
        if( stateHolder is not MainRoboNpc robo ) return;

        var robo_pos = robo.GlobalPosition;

        robo.RoboAnimator.HeadLookAtPosition = robo.WaypointHandler.CurrentWaypoint;
        robo.RoboAnimator.BodyLookAtPosition = robo.WaypointHandler.CurrentWaypoint;
        robo.MoveDirection = robo.WaypointHandler.GetWalkDirection( robo_pos );

        robo.WaypointHandler.UpdateWaypoint( robo_pos );

        if( robo.WaypointHandler.waypointType != WaypointHandler.eWaypointType.NONE ) return;

        ChangeState( stateHolder, ROBO_IDLE_STATE );
    }
}