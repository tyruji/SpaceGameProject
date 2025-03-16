using System;
using Godot;

public partial class StepStompElement : Node3D, ISteppable, IStompable, IOrientationSpace
{
    [Signal]
    public delegate void OnStepEventHandler();

    [Signal]
    public delegate void OnStompEventHandler();

    [Export]
    public float StepCooldown { get; set; } = .5f;

    [Export]
    public float StompCooldown { get; set; } = .5f;

    private float _stepElapsed = .0f;

    private float _stompElapsed = .0f;

    public override void _Process( double delta )
    {
        float dt = ( float ) delta;
        _stepElapsed += dt;
        _stompElapsed += dt;
    }

    public void StepOn()
    {
        if( _stepElapsed < StepCooldown ) return;
        _stepElapsed = 0f;

        EmitSignal( SignalName.OnStep );
    }

    public void StompOn()
    {
        if( _stompElapsed < StompCooldown ) return;
        _stompElapsed = 0f;

        EmitSignal( SignalName.OnStomp );
    }
}