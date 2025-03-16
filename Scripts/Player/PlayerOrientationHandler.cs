using Godot;
using System;

public partial class PlayerOrientationHandler : Node3D
{
    [Export]
    public float OrientationCorrectionSpeed { get; set; } = 1.2f;

    [Export]
    public Player Player { get; set; }

    [Export]
    public RayCast3D RayCast { get; set; }

    public Transform3D targetTransform = default;

    public override void _Ready()
    {
        Player ??= GetParent<Player>();
        RayCast ??= GetNode<RayCast3D>( nameof( RayCast3D ) );
    }

    public override void _Process( double delta )
    {
        HandleRay();

        // var tr = Player.Transform;
        // tr.Basis.Y = targetTransform.Basis.Y;
        // Player.Transform = tr;

        // Vector3 player_up = Player.Transform.Basis.Y;
        // Vector3 target_up = targetTransform.Basis.Y;

        // // //Vector3 target_rot = .5f * Mathf.Pi * target_up.Cross( player_up ) - Player.Rotation;
        // Vector3 target_rot = target_up.Cross( player_up );
        // float alpha = ( float ) delta * OrientationCorrectionSpeed;
        // //Player.Rotation = Player.Rotation.Lerp( target_rot, alpha );
        // Player.Rotation += target_rot * alpha;
    }

    private void HandleRay()
    {
        if( !RayCast.IsColliding() ) return;

        var col = RayCast.GetCollider();

        if( col is not IOrientationSpace orientationSpace ) return;
        
        if( targetTransform == orientationSpace.Transform ) return;

        targetTransform = orientationSpace.Transform;
        UpdatePlayerBasis();
    }

    private void UpdatePlayerBasis()
    {
        var p_up = Player.GlobalBasis.Y;
        var t_up = targetTransform.Basis.Y;
        Player.UpDirection = t_up;

        if( p_up.IsEqualApprox( t_up ) ) return;
        
        var b = Player.GlobalBasis;
        var n = t_up.Cross( p_up ).Normalized();

            // For some fucking reason the dot product ends up exceeding 1...
            // what the fuck? both vectors are normalised!!!
            // anyway i ended up clamping this.
        var angle = Mathf.Acos( Mathf.Clamp( p_up.Dot( t_up ), -1f, 1f ) );
        Player.GlobalBasis = b.Rotated( n, -angle ).Orthonormalized();
    }
}
