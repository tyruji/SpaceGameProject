using Godot;
using System;

public partial class Door : Node3D
{
    [Export]
    public RandomizedSoundSettings3D OpenSound { get; set; }

    [Export]
    public RandomizedSoundSettings3D CloseSound { get; set; }
    
    [Export]
    public AnimationPlayer AnimationPlayer = null;

    public override void _Ready()
    {
        AnimationPlayer ??= GetNode<AnimationPlayer>( nameof( AnimationPlayer ) );
    }

    public void Open()
    {
        AnimationPlayer.Play( nameof( Open ) );

        if( OpenSound == null ) return;

        AudioManager.Instance.Play3D( OpenSound, GlobalPosition );
    }

    public void Close()
    {
        AnimationPlayer.Play( nameof( Close ) );

        if( CloseSound == null ) return;

        AudioManager.Instance.Play3D( CloseSound, GlobalPosition );
    }
}
