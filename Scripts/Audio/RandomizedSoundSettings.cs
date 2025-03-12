using Godot;

[GlobalClass]
public partial class RandomizedSoundSettings : Resource, ISoundSettings
{
    [Export]
    public AudioStream Sound { get; set; }

    [Export]
    public float MinVolume { get; set; } = -1;

    [Export]
    public float MaxVolume { get; set; } = -1;

    [Export]
    public float MinPitch { get; set; } = .9f;

    [Export]
    public float MaxPitch { get; set; } = 1.1f;

    public float Volume => ( float ) GD.RandRange( MinVolume, MaxVolume );

    public float Pitch => ( float )GD.RandRange( MinPitch, MaxPitch );
}