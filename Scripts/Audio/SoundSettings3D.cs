using Godot;

[GlobalClass]
public partial class SoundSettings3D : SoundSettings, ISoundSettings3D
{
    [Export]
    public float UnitSize { get; set; } = 12f;
}