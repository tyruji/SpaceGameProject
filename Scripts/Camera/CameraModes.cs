using Godot;

public static class CameraModes
{
    public static readonly CameraSmoothFollowMode CAMERA_SMOOTH_FOLLOW_MODE = new CameraSmoothFollowMode();

    public static readonly CameraTightFollowMode CAMERA_TIGHT_FOLLOW_MODE = new CameraTightFollowMode();
}

public class CameraSmoothFollowMode : ICameraMode
{
    public Node3D FollowTarget { get; set; }
    public float RotationDampening { get; set; } = 12f;
    public float PositionDampening { get; set; } = 18f;

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