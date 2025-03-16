using Godot;
using System;

public partial class SpaceShip : RigidBody3D, IControllable, IInteractable, IOrientationSpace
{
    [Export]
    public float MaxSpeed { get; set; } = 20f;

    [Export]
    public float CruiseSpeed { get; set; } = 1f;

    [Export]
    public float Acceleration { get; set; } = 10f;

    [Export]
    public float Deacceleration { get; set; } = 7f;

    [Export]
    public float RotationSpeed { get; set; } = .5f;

    [Export]
    public float RotationStabilisationSpeed { get; set; } = 1f;

    [Export]
    public float RotationAcceleration { get; set; } = 15f;

    [Export]
    public float RotationDeacceleration { get; set; } = 5f;

    [Export]
    public float TiltSpeed { get; set; } = .3f;

    [Export]
    public float TiltAcceleration { get; set; } = 15f;

    [Export]
    public float HoverSpeed { get; set; } = 10f;

    [Export]
    public Node3D CameraPivot { get; set; }

    [Export]
    public SpringArm3D SpringArm { get; set; }

    [Export]
    public Node3D CameraFollowPoint { get; set; }

    [Export]
    public RayCast3D CheckHoverRayCast { get; set; }

    [Export]
    public RayCast3D CheckGroundRayCast { get; set; }

    public bool EnableControl
    {
        get => _enableControl;
        set
        {
            _enableControl = value;
            CustomIntegrator = value;
            
            // PhysicsServer3D.BodySetOmitForceIntegration( GetRid(), value );
        }
    }
    private bool _enableControl = false;

    public Vector2 rotationDirection = Vector2.Zero;

    public float forwardInput = 0f;

    public ICameraMode CameraMode
    {
        get
        {
            var mode = CameraModes.CAMERA_SMOOTH_FOLLOW_MODE;
            mode.FollowTarget = CameraFollowPoint;
            return mode;
        }
    }

    public Vector3 velocity = Vector3.Zero;

    public Vector3 hoverVelocity = Vector3.Zero;

    public Vector3 angularVelocity = Vector3.Zero;

    public Vector3 tiltAngularVelocity = Vector3.Zero;

    public override void _Ready()
    {
        CameraPivot ??= GetNode<Node3D>( nameof( CameraPivot ) );
        SpringArm ??= CameraPivot.GetNode<SpringArm3D>( nameof( SpringArm3D ) );
        CameraFollowPoint ??= SpringArm.GetNode<Node3D>( nameof( CameraFollowPoint ) ); 
        CheckHoverRayCast ??= GetNode<RayCast3D>( nameof( CheckHoverRayCast ) );
        CheckGroundRayCast ??= GetNode<RayCast3D>( nameof( CheckGroundRayCast ) );
    }

    public override void _UnhandledInput( InputEvent @event )
    {
        if( !EnableControl ) return;

        rotationDirection = Input.GetVector( InputActions.LEFT, InputActions.RIGHT,
                                          InputActions.BACK, InputActions.FORWARD );

        forwardInput = Input.GetAxis( InputActions.VEHICLE_BACK, InputActions.VEHICLE_FORWARD );

            // Freeing the mouse
        if( Input.IsActionJustPressed( InputActions.CANCEL ) )
        {
            Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Visible
                ? Input.MouseModeEnum.Captured : Input.MouseModeEnum.Visible;
        }
    }

    public override void _IntegrateForces( PhysicsDirectBodyState3D state )
    {
        if( !EnableControl )
        {
            HandleTurnOff( state );
            return;
        }

        float acc = Deacceleration;
        float speed_target = CruiseSpeed;
        
        if( forwardInput != 0 )
        {
            acc = Acceleration;
            speed_target = forwardInput * MaxSpeed;
        }
        
        Vector3 fwd = -Transform.Basis.Z;
        Vector3 up = Transform.Basis.Y;
        Vector3 right = Transform.Basis.X;

        hoverVelocity = hoverVelocity.Lerp( GetHoverVelocity(), state.Step * acc );
        velocity = velocity.Lerp( fwd * speed_target, acc * state.Step );
        state.LinearVelocity = velocity + hoverVelocity;
        
        float rot_acc = RotationDeacceleration;
        Vector3 rot_vel = Vector3.Zero;

        if( rotationDirection.LengthSquared() > 0 )
        {
            rot_acc = RotationAcceleration;
            rot_vel -= RotationSpeed * rotationDirection.X * up;
            rot_vel -= RotationSpeed * rotationDirection.Y * right;
        }

            // This gradually removes tilt
        rot_vel += RotationStabilisationSpeed * right.Y * fwd;

        angularVelocity = angularVelocity.Lerp( rot_vel, state.Step * rot_acc );
        
        var tilt_vel = rotationDirection.X * fwd * TiltSpeed;
        tiltAngularVelocity = tiltAngularVelocity.Lerp( tilt_vel, state.Step * TiltAcceleration );
        
        state.AngularVelocity = angularVelocity + tiltAngularVelocity;

        // var target_rot = Vector3.Zero;
        // target_rot.Y = Rotation.Y;
        // var target_vel = target_rot - Rotation;
    }

        // Store the player here, their position relative to the ship,
        // and switch control over to them, when the player leaves the ship.
    public void Interact( Node interactor ) {}

    private void HandleTurnOff( PhysicsDirectBodyState3D state )
    {
        GravityScale = CheckGroundRayCast.IsColliding() ? 1f : 0f;
    }

    private Vector3 GetHoverVelocity()
    {
        if( !CheckHoverRayCast.IsColliding() ) return Vector3.Zero;
        var col_point = CheckHoverRayCast.GetCollisionPoint();
        var ray_len_sqr = CheckHoverRayCast.TargetPosition.LengthSquared();
        var ray_to_col = col_point - CheckHoverRayCast.GlobalPosition;
        
        float scale = 1f - ray_to_col.LengthSquared() / ray_len_sqr;
        
        if( scale < 0f ) return Vector3.Zero;

        return HoverSpeed * scale * CheckHoverRayCast.GetCollisionNormal();  
    }
}
