using Godot;
using System;


/*
    ********************************************************************    
                    TODO:
    IF THE RAY IS NOT DETECTING ANY GROUND,
    PERFORM A SPHERE CAST TO CHECK FOR THE CLOSEST ORIENTATION SPACE
    ALSO WOULD BE NICE TO DETECT IF THE PLAYER IS IN SPACE
    BECAUSE THEN THERE WOULD BE NO GRAVITY,
    SO LIKE, WHEN ENTERING PLANETS MAKE SOMETHING LIKE A BOOL IN_SPACE = FALSE
    AND WHEN LEAVING A PLANET IN_SPACE = TRUE, DEFAULTING TO NO GRAVITY IF 
    currentSpace IS NULL.

    ********************************************************************
*/


public partial class PlayerOrientationHandler : Node3D
{
    [Export]
    public float OrientationCorrectionTime { get; set; } = 1.2f;

    [Export]
    public Player Player { get; set; }

    [Export]
    public RayCast3D RayCast { get; set; }

    public Transform3D targetTransform = default;

    public Vector3 rotationAxis = Vector3.Zero;

    public float rotationAngle = 0f;

    public float timeElapsed = 0f;

    public IOrientationSpace currentSpace = null;

    public override void _Ready()
    {
        Player ??= GetParent<Player>();
        RayCast ??= GetNode<RayCast3D>( nameof( RayCast3D ) );
    }

    public override void _Process( double delta )
    {
        HandleRay();

        if( timeElapsed > OrientationCorrectionTime || currentSpace == null ) return;
        float dt = ( float ) delta;
        timeElapsed += dt;

        var p_b = Player.Basis;
        var angle_diff = rotationAngle * dt / OrientationCorrectionTime;
        p_b = p_b.Rotated( rotationAxis, angle_diff );
        Player.Basis = p_b.Orthonormalized();

        if( timeElapsed <= OrientationCorrectionTime ) return;
    
            // Do this at the end of interpolation
            // to ensure we rotated fully,
            // without small error.
        UpdateCharacterBasis();
        Player.Basis = Player.Basis.Rotated( rotationAxis, rotationAngle );
    }

    private void HandleRay()
    {
        if( !RayCast.IsColliding() )
        {
            currentSpace = null;
        }

        var col = RayCast.GetCollider();

        if( col is not IOrientationSpace orientationSpace ) return;
        currentSpace = orientationSpace;

        if( targetTransform == orientationSpace.Transform ) return;

        targetTransform = orientationSpace.Transform;
        UpdateCharacterBasis();
        timeElapsed = 0f;
    }

    private void UpdateCharacterBasis()
    {
        var p_up = Player.GlobalBasis.Y;
        var t_up = targetTransform.Basis.Y.Normalized();
        Player.UpDirection = t_up;

        if( p_up.IsEqualApprox( t_up ) ) return;
        
        rotationAxis = t_up.Cross( p_up ).Normalized();

            // For some fucking reason the dot product ends up exceeding 1...
            // what the fuck? both vectors are normalised!!!
            // anyway i ended up clamping this.
        rotationAngle = -Mathf.Acos( Mathf.Clamp( p_up.Dot( t_up ), -1f, 1f ) );
    }
}
