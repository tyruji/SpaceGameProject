using Godot;
using System;

public partial class AutomaticDoor : Door
{
    [Export]
    public bool OnlyDetectCharacters { get; set; } = true;

    private int _bodiesInRange = 0;

    private bool _closed = true;

    public override void _Process( double delta )
    {
        if( AnimationPlayer.IsPlaying() ) return;

        if( _bodiesInRange == 0 && !_closed )
        {
            Close();
            _closed = true;
        }
        else if( _bodiesInRange > 0 && _closed )
        {
            Open();
            _closed = false;
        }
    }

    public void OnBodyEntered( Node body )
    {
        if( OnlyDetectCharacters && body is not ICharacter ) return;

            // Only detect moving bodies.
        //if( body is not CharacterBody3D and not RigidBody3D ) return;

        ++_bodiesInRange;
    }

    public void OnBodyExited( Node body )
    {
        if( OnlyDetectCharacters && body is not ICharacter ) return;

        //if( body is not CharacterBody3D and not RigidBody3D ) return;

        --_bodiesInRange;
    }
}
