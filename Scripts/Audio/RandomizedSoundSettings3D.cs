using Godot;

[GlobalClass]
public partial class RandomizedSoundSettings3D : RandomizedSoundSettings, ISoundSettings3D
{
    [Export]
    public float MinUnitSize { get; set; } = 8f;

    [Export]
    public float MaxUnitSize { get; set; } = 12f;

    public float UnitSize => ( float )GD.RandRange( MinUnitSize, MaxUnitSize ); 
}