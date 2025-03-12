using Godot;
using System;

public partial class AudioPlayerRandomized : AudioStreamPlayer
{
    [Export]
    public RandomizedSoundSettings SoundSettings { get; set; }

    public void PlayAudio()
    {
        PitchScale = SoundSettings.Pitch;
        VolumeDb = SoundSettings.Volume;
        Play();
    }
}
