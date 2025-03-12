using Godot;
using System;

public partial class Head : Node3D
{
    [Export]
    public float HeadBobAmplitude { get; set; } = .2f;

    [Export]
    public float HeadBobFrequency { get; set; } = 20f;

    [Export]
    public float HeadBobDampening { get; set; } = 10f;

    [Export]
    public Player Player { get; set; }

    [Export]
    public RayCast3D RayCast { get; set; }

    public IInteractable selectedInteractable = null;

    public IHighlightable selectedHighlightable = null;

    public Vector3 initialHeadPosition = Vector3.Zero;

    private eHeadBobState _headBobState = eHeadBobState.Idle;

    private float _timeElapsed = 0f;

    public override void _Ready()
    {
        Player ??= GetParent<Player>();
        RayCast ??= GetNode<RayCast3D>( nameof( RayCast3D ) );

        initialHeadPosition = Position;
    }

    public override void _UnhandledInput( InputEvent @event )
    {
        if( !Player.EnableControl ) return;

        if( !Input.IsActionJustPressed( InputActions.INTERACT ) ) return;

        if( selectedInteractable == null ) return;
        Player.NotifyInteraction( selectedInteractable );
    }

    public override void _Process( double delta )
    {
        HandleRayCast();
        HeadBob( delta );
    }

    private void HandleRayCast()
    {
        var col = RayCast.GetCollider();

        if( col is IInteractable interactable )
        {
            selectedInteractable = interactable;
        }
        else
        {
            selectedInteractable = null;
        }
    
        if( col is not IHighlightable highlightable || !Player.EnableControl )
        {
            selectedHighlightable?.Unhighlight();
            selectedHighlightable = null;
            return;
        }

        if( highlightable == selectedHighlightable ) return;

        selectedHighlightable?.Unhighlight();
        highlightable.Highlight();
        selectedHighlightable = highlightable;
    }

    private void HeadBob( double delta )
    {
        float dt = ( float ) delta;

        switch( _headBobState )
        {
            case eHeadBobState.Idle:
                Position = Position.Lerp( initialHeadPosition, dt * HeadBobDampening );

                if( Player.IsOnFloor() && Player.inputDirection.LengthSquared() > 0 )
                {
                    _headBobState = eHeadBobState.Bobbing;
                }
            break;

            case eHeadBobState.Bobbing:
                Vector3 offset = Vector3.Zero;
                
                // offset.Y += Mathf.Lerp( offset.Y, 
                //     Mathf.Sin( _timeElapsed * HeadBobFrequency ) * HeadBobAmplitude * 1.4f,
                //     HeadBobDampening * dt );
                // offset.X += Mathf.Lerp( offset.X, 
                //     Mathf.Sin( .5f * _timeElapsed * HeadBobFrequency ) * HeadBobAmplitude * 1.6f,
                //     HeadBobDampening * dt );
                
                // Position += offset;
                
                offset.Y = Mathf.Sin( _timeElapsed * HeadBobFrequency );
                offset.X = Mathf.Cos( .5f * _timeElapsed * HeadBobFrequency );
                offset *= HeadBobAmplitude;

                Position = Position.Lerp( initialHeadPosition + offset, dt * HeadBobDampening );
                _timeElapsed += dt;

                if( !Player.IsOnFloor() || Player.inputDirection.LengthSquared() <= 0 )
                {
                    _headBobState = eHeadBobState.Idle;
                    _timeElapsed = 0f;
                }

            break;
        }
    }

    enum eHeadBobState
    {
        Idle,
        Bobbing
    }
}
