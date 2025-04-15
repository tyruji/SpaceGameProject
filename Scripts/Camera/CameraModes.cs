using Godot;

public static class CameraModes
{
    public static readonly CameraNoneMode CAMERA_NONE_MODE = new CameraNoneMode();

    public static readonly CameraSmoothFollowMode CAMERA_SMOOTH_FOLLOW_MODE = new CameraSmoothFollowMode();

    public static readonly CameraTightFollowMode CAMERA_TIGHT_FOLLOW_MODE = new CameraTightFollowMode();

    public static readonly CameraSmoothFollowTransitionToOtherMode CAMERA_SMOOTH_TO_TRANSITION_MODE = new CameraSmoothFollowTransitionToOtherMode();

    public static readonly CameraTightSmoothBasisChangeMode CAMERA_TIGHT_BASIS_CHANGE_SMOOTH_MODE = new CameraTightSmoothBasisChangeMode();
}

public class CameraNoneMode : ICameraMode
{
    public void Handle( CameraHandler cameraHandler, double delta ) {}
}

public class CameraSmoothFollowMode : ICameraMode
{
    public Node3D FollowTarget { get; set; }
    public float RotationDampening { get; set; } = 6f;
    public float PositionDampening { get; set; } = 10f;

    public void Handle( CameraHandler cameraHandler, double delta )
    {
        if( FollowTarget == null ) return;

        float dt = ( float ) delta;

        var global_tr = cameraHandler.GlobalTransform.Orthonormalized();
        var target_tr = FollowTarget.GlobalTransform;

        global_tr.Basis = global_tr.Basis.Slerp( target_tr.Orthonormalized().Basis,
            RotationDampening * dt );

        global_tr.Origin = global_tr.Origin.Lerp( target_tr.Origin, PositionDampening * dt );

        cameraHandler.GlobalTransform = global_tr;
    }
}

public class CameraSmoothFollowTransitionToOtherMode : ICameraMode
{
    public Node3D FollowTarget { get; set; }
    public float RotationDampening { get; set; } = 6f;
    public float PositionDampening { get; set; } = 10f;
    public float MinDistanceToTargetForTransition { get; set; } = .2f;
    public ICameraMode NextMode { get; set; } = CameraModes.CAMERA_TIGHT_FOLLOW_MODE;

    public void Handle( CameraHandler cameraHandler, double delta )
    {
        if( FollowTarget == null ) return;

        float dt = ( float ) delta;

        var global_tr = cameraHandler.GlobalTransform.Orthonormalized();
        var target_tr = FollowTarget.GlobalTransform;

        global_tr.Basis = global_tr.Basis.Slerp( target_tr.Orthonormalized().Basis,
            RotationDampening * dt );

        global_tr.Origin = global_tr.Origin.Lerp( target_tr.Origin, PositionDampening * dt );

        cameraHandler.GlobalTransform = global_tr;

        var distance = cameraHandler.GlobalPosition - FollowTarget.GlobalPosition;
        var min_d = MinDistanceToTargetForTransition;
        if( distance.LengthSquared() >= min_d * min_d ) return;

        cameraHandler.CameraMode = NextMode;
    }
}

public class CameraTightSmoothBasisChangeMode : ICameraMode
{
    public Node3D FollowTarget { get; set; }
    public Node3D BasisDetectNode { get; set; }
    public float RotationDampening { get; set; } = 6f;
    public float MinDistanceToTargetForTransition { get; set; } = .5f;

    public void Handle( CameraHandler cameraHandler, double delta )
    {
        if( FollowTarget == null ) return;

        var cam_basis = cameraHandler.GlobalBasis;
        var t_basis = BasisDetectNode.GlobalBasis;

        var dist = cam_basis.Y - t_basis.Y;

        var min_d = MinDistanceToTargetForTransition;

        if( dist.LengthSquared() < min_d * min_d )
        {
            cameraHandler.GlobalBasis = FollowTarget.GlobalBasis;
            cameraHandler.GlobalPosition = FollowTarget.GlobalPosition;
            return;
        }

        float dt = ( float ) delta;

        var global_tr = cameraHandler.GlobalTransform.Orthonormalized();
        var target_tr = FollowTarget.GlobalTransform;

        global_tr.Basis = global_tr.Basis.Slerp( target_tr.Orthonormalized().Basis,
            RotationDampening * dt );

        cameraHandler.GlobalTransform = global_tr;
        cameraHandler.GlobalPosition = FollowTarget.GlobalPosition;
    }
}

public class CameraTightFollowMode : ICameraMode
{
    public Node3D FollowTarget { get; set; }

    public void Handle( CameraHandler cameraHandler, double delta )
    {
        if( FollowTarget == null ) return;

        cameraHandler.GlobalBasis = FollowTarget.GlobalBasis;
        cameraHandler.GlobalPosition = FollowTarget.GlobalPosition;
    }
}