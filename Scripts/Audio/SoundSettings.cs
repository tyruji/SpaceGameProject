using Godot;

[GlobalClass]
public partial class SoundSettings : Resource, ISoundSettings
{
    [Export]
    public AudioStream Sound { get; set; }

    [Export]
    public float Volume { get; set; } = .0f;
    
    [Export]
    public float Pitch { get; set; } = 1.0f;
}


