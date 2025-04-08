using Godot;
using System;

public partial class RoboAnimator : Node3D
{
    [Export]
    public RoboModel Model { get; set; }

    [Export]
    public float HeadRotationPeriod { get; set; } = .7f;

    [Export]
    public float HeadRotationTime { get; set; } = .6f;

    public float timeElapsed = 0;

    public override void _Ready()
    {
        Model ??= GetParent().GetNode<RoboModel>( nameof( Model ) );
    }

    public override void _Process( double delta )
    {
        var offset = .01f * MathHelper.FastRandomPointInUnitSphere();
        Model.Body.Position = offset;
        
        Model.Head.Position = Model.HeadBasePosition - offset;

        timeElapsed += ( float ) delta;
        if( timeElapsed < HeadRotationPeriod ) return;
        timeElapsed = 0;

        var player_pos = GetNode<Node3D>( "../../Player" ).GlobalPosition;
        // Model.Head.LookAt( player_pos, null, true );
        var local_player_pos = Model.ToLocal( player_pos );
        var tr = Model.Head.Transform;
        tr.Basis = Basis.LookingAt( local_player_pos, null, true );
        
        var tween = CreateTween();
        tween.TweenProperty( Model.Head, "basis", tr.Basis, HeadRotationTime )
                                .SetTrans( Tween.TransitionType.Elastic )
                                .SetEase( Tween.EaseType.InOut );
    }
}
