using System;
using Godot;

public partial class StepStompElementAudio : StepStompElement
{
    [Export]
    public RandomizedSoundSettings StepSoundSettings { get; set; }

    [Export]
    public RandomizedSoundSettings StompSoundSettings { get; set; }

    public override void _Ready()
    {
        base._Ready();
    
        Connect( SignalName.OnStep, Callable.From( PlayStep ) );

        Connect( SignalName.OnStomp, Callable.From( PlayStomp ) );
    }

    public void PlayStep()
    {
        AudioManager.Instance.Play( StepSoundSettings );
    }

    public void PlayStomp()
    {
        AudioManager.Instance.Play( StompSoundSettings );
    }
}