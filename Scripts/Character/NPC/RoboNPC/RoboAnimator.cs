using Godot;
using System;

public partial class RoboAnimator : Node3D
{
    [Export]
    public RoboModel Model { get; set; }

    [Export]
    public float HeadRotationTime { get; set; } = .6f;

    [Export]
    public float BodyRotationTime { get; set; } = .3f;

    public Vector3 HeadLookAtPosition 
    {
        get => _headLookAtPosition;
        set
        {
            _headLookAtPosition = value;
            _updateHeadLookAt = true;
        }
    }
    private bool _updateHeadLookAt = false;
    private Vector3 _headLookAtPosition = Vector3.Zero;

    public Vector3 BodyLookAtPosition
    {
        get => _bodyLookAtPosition;
        set
        {
            _bodyLookAtPosition = value;
            _updateBodyLookAt = true;
        }
    }
    private bool _updateBodyLookAt = false;
    private Vector3 _bodyLookAtPosition = Vector3.Zero;

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

        var local_pos = Model.ToLocal( _headLookAtPosition );
        var basis = Basis.LookingAt( local_pos, null, true );
        
        var tween = CreateTween();
        tween.TweenProperty( Model.Head, "basis", basis, HeadRotationTime )
                                .SetTrans( Tween.TransitionType.Elastic )
                                .SetEase( Tween.EaseType.InOut );
    }

    private void UpdateBodyLookAtAnimation()
    {
        if( !_updateBodyLookAt ) return;
        _updateBodyLookAt = false;

        var local_pos = Model.ToLocal( _bodyLookAtPosition );
        var basis = Basis.LookingAt( local_pos, null, true );
        
        var tween = CreateTween();
        tween.TweenProperty( Model.Body, "basis", basis, BodyRotationTime )
                                .SetTrans( Tween.TransitionType.Circ )
                                .SetEase( Tween.EaseType.InOut );
    }
}
