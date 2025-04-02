public interface IStateHolder
{
    State State { get; set; }
}

public abstract class State
{
    public virtual void Enter( IStateHolder stateHolder ) {}

    public virtual void Exit( IStateHolder stateHolder ) {}
    public abstract void Handle( IStateHolder stateHolder, object arg = null );

    public void ChangeState( IStateHolder stateHolder, State nextState )
    {
        stateHolder.State.Exit( stateHolder );
        stateHolder.State = nextState;
        nextState.Enter( stateHolder );
    }
}

