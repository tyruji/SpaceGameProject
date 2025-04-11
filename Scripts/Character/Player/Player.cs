using System;
using Godot;

public partial class Player : CharacterBody3D, IControllable, ICharacter
{
    public event Action<IInteractable> OnInteract;

    [Export]
    public float Gravity { get; set; } = 24f;

    [Export]
    public float MaxSpeed { get; set; } = 20f;

    [Export]
    public float JumpSpeed { get; set; } = 18f;

    [Export]
    public float Acceleration { get; set; } = 4.5f;

    [Export]
    public float Friction { get; set; } = 16;

    [Export]
    public float HeadRotationMaxAngle { get; set; } = 70;

    [Export]
    public float MouseSensitivity { get; set; } = .12f;

    [Export]
    public bool WantsToJump { get; set; } = false;

    [Export]
    public Node3D Head { get; set; }

    public bool EnableControl 
    {
        get => _enableControl;
        set
        {
            _enableControl = value;
            if( !value )
            {
                this.ProcessMode = ProcessModeEnum.Disabled;
            }
            else
            {
                this.ProcessMode = ProcessModeEnum.Inherit;
            }
        }
    }
    private bool _enableControl = false;

    public ICameraMode CameraMode
    {
        get
        {
            var mode = CameraModes.CAMERA_TIGHT_FOLLOW_MODE;
            mode.FollowTarget = Head;
            
            return mode;
        }
    }

    public Vector3 MoveDirection { get; set; } = Vector3.Zero;

    public override void _Ready()
    {
        Head ??= GetNode<Node3D>( nameof( Head ) );
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _UnhandledInput( InputEvent @event )
    {
        if( !EnableControl ) return;

        HandleMouseRotationInput( @event );
        HandleMovementInput( @event );
    }

    public override void _PhysicsProcess( double delta )
    {
        if( !EnableControl ) return;

        this.HandleBaseMovement( delta );
        MoveAndSlide();
    }

    public void NotifyInteraction( IInteractable interactable )
    {
        OnInteract?.Invoke( interactable );
        interactable.Interact( this );
    }

    private void HandleMouseRotationInput( InputEvent @event )
    {
        if( @event is not InputEventMouseMotion mouse_event ) return;
        if( Input.MouseMode != Input.MouseModeEnum.Captured ) return;

            // Rotate body and head with mouse.
        Head.RotateX( Mathf.DegToRad( -mouse_event.Relative.Y * MouseSensitivity ) );
            // up direction changes based on the environment (planets, spaceships, structures)
        var up = GlobalBasis.Y;
        Rotate( up, Mathf.DegToRad( -mouse_event.Relative.X * MouseSensitivity ) );

            // Clamp up and down head rotation
        var head_rot = Head.RotationDegrees;
        head_rot.X = Mathf.Clamp( head_rot.X, -HeadRotationMaxAngle, HeadRotationMaxAngle );
        Head.RotationDegrees = head_rot; 
    }

    private void HandleMovementInput( InputEvent @event )
    {
        MoveDirection = Vector3.Zero;
        
        Vector2 dir = Input.GetVector( InputActions.LEFT, InputActions.RIGHT,
            InputActions.FORWARD, InputActions.BACK );

        MoveDirection += dir.X * GlobalBasis.X.Normalized();
        MoveDirection += dir.Y * GlobalBasis.Z.Normalized();

        WantsToJump = IsOnFloor() && Input.IsActionJustPressed( InputActions.JUMP );

        if( Input.IsActionJustPressed( InputActions.CANCEL ) )
        {
            Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Visible
                ? Input.MouseModeEnum.Captured : Input.MouseModeEnum.Visible;
        }
    }
}