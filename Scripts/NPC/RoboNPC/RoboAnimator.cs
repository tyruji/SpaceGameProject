using Godot;
using System;

public partial class RoboAnimator : Node3D
{
    [Export]
    public RoboModel Model { get; set; }

    [Export]
    public float HeadRotationTime { get; set; } = .6f;

    public Vector3 LookAtPosition 
    {
        get => _lookAtPosition;
        set
        {
            _lookAtPosition = value;
            _updateHeadLookAt = true;
        }
    }

    private bool _updateHeadLookAt = false;

    private Vector3 _lookAtPosition = Vector3.Zero;

    public override void _Ready()
    {
        Model ??= GetParent().GetNode<RoboModel>( nameof( Model ) );
    }

    public override void _Process( double delta )
    {
        BodyShake();
    }

    public void BodyShake()
    {
        var offset = .01f * MathHelper.FastRandomPointInUnitSphere();
        Model.Body.Position = offset;
        
        Model.Head.Position = Model.HeadBasePosition - offset;
    }

    private void UpdateHeadLookAtAnimation()
    {
        if( !_updateHeadLookAt ) return;
        _updateHeadLookAt = false;

        var local_pos = Model.ToLocal( _lookAtPosition );
        var basis = Basis.LookingAt( local_pos, null, true );
        
        var tween = CreateTween();
        tween.TweenProperty( Model.Head, "basis", basis, HeadRotationTime )
                                .SetTrans( Tween.TransitionType.Elastic )
                                .SetEase( Tween.EaseType.InOut );
    }
}
