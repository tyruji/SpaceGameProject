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

    public bool Closed { get; private set; } = true;

    public override void _Ready()
    {
        AnimationPlayer ??= GetNode<AnimationPlayer>( nameof( AnimationPlayer ) );
    }

    public void Open()
    {
        if( !Closed ) return;

        Closed = false;
        AnimationPlayer.Play( nameof( Open ) );

        if( OpenSound == null ) return;

        AudioManager.Instance.Play3D( OpenSound, GlobalPosition );
    }

    public void Close()
    {
        if( Closed ) return;

        Closed = true;
        AnimationPlayer.Play( nameof( Close ) );

        if( CloseSound == null ) return;

        AudioManager.Instance.Play3D( CloseSound, GlobalPosition );
    }
}
