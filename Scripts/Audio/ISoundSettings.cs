using Godot;

public interface ISoundSettings
{
    AudioStream Sound { get; }
    float Volume { get; }
    float Pitch { get; }
}

public interface ISoundSettings3D : ISoundSettings
{
    float UnitSize { get;}
}