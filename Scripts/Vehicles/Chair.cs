using Godot;
using System;

public partial class Chair : StaticBody3D, IControllable, IInteractable, IHighlightable
{
    [Export]
    private Marker3D CameraPositionMarker { get; set; }

    [Export]
    private MeshInstance3D HighlightMesh { get; set; }

    public bool EnableControl { get; set; }

    public bool InteractionEnabled { get; set; } = true;

    public ICameraMode CameraMode
    {
        get
        {
            var mode = CameraModes.CAMERA_SMOOTH_FOLLOW_MODE;
            mode.FollowTarget = CameraPositionMarker;
            return mode;
        }
    }

    public override void _Ready()
    {
        CameraPositionMarker ??= GetNode<Marker3D>( nameof( CameraPositionMarker ) );
        HighlightMesh ??= GetNode<MeshInstance3D>( nameof( HighlightMesh ) );
    }

    public override void _UnhandledInput( InputEvent @event )
    {
        if( !EnableControl ) return;

            // Freeing the mouse
        if( Input.IsActionJustPressed( InputActions.CANCEL ) )
        {
            Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Visible
                ? Input.MouseModeEnum.Captured : Input.MouseModeEnum.Visible;
        }
    }

    public void Interact( Node interactor ) {}

    public void Highlight()
    {
        HighlightMesh?.Show();
    }

    public void Unhighlight()
    {
        HighlightMesh?.Hide();
    }
}
