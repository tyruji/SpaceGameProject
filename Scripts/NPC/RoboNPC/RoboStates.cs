public static class RoboStates
{
    public static readonly RoboIdleState ROBO_IDLE_STATE = new RoboIdleState();
}

public abstract class RoboState : State {}

public class RoboIdleState : RoboState
{
    public override void Handle( IStateHolder stateHolder, object arg = null )
    {
        if( stateHolder is not MainRoboNpc robo ) return;

        robo.RoboAnimator.LookAtPosition = robo.Player.GlobalPosition;
    }
}

public class RoboWalkToPointState : RoboState
{
    public override void Handle( IStateHolder stateHolder, object arg = null )
    {
        if( stateHolder is not MainRoboNpc robo ) return;

        robo.RoboAnimator.LookAtPosition = robo.Player.GlobalPosition;
    }
}