using Godot;
using System;

public partial class Door : StaticBody3D
{
    [Export]
    public RandomizedSoundSettings3D OpenSound { get; set; }

    [Export]
    public RandomizedSoundSettings3D CloseSound { get; set; }
    
    protected AnimationPlayer animationPlayer = null;

    public override void _Ready()
    {
        animationPlayer ??= GetNode<AnimationPlayer>( nameof( AnimationPlayer ) );
    }

    public void Open()
    {
        animationPlayer.Play( nameof( Open ) );

        if( OpenSound == null ) return;

        AudioManager.Instance.Play3D( OpenSound, GlobalPosition );
    }

    public void Close()
    {
        animationPlayer.Play( nameof( Close ) );

        if( CloseSound == null ) return;

        AudioManager.Instance.Play3D( CloseSound, GlobalPosition );
    }
}
