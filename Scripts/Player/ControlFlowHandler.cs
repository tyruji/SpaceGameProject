using Godot;
using System;

public partial class ControlFlowHandler : Node
{
    [Export]
    public Player Player { get; set; }

    [Export]
    public CameraHandler CameraHandler { get; set;}

    public IControllable currentControllable = null;

    public Vector3 playerRelativePosition = Vector3.Zero;

    public Vector3 playerRelativeRotation = Vector3.Zero;

    public override void _Ready()
    {
        Player ??= GetParent<Player>();
        CameraHandler ??= Player.GetParent().GetNode<CameraHandler>( nameof( CameraHandler ) );

        Player.OnInteract += OnInteract;

        SwitchControlTo( Player );
    }

    public override void _UnhandledInput( InputEvent @event )
    {
        if( !Input.IsActionJustPressed( InputActions.VEHICLE_EXIT ) ) return;
        
        SwitchControlBackToPlayer();
    }

    public override void _ExitTree()
    {
        Player.OnInteract -= OnInteract;
    }

    public void SwitchControlBackToPlayer()
    {
        if( currentControllable is not Node3D node3D || currentControllable == Player ) return;

        Player.GlobalPosition = node3D.ToGlobal( playerRelativePosition );
        Player.GlobalRotation = node3D.GlobalRotation + playerRelativeRotation;
        SwitchControlTo( Player );
    }

    private void OnInteract( IInteractable interactable )
    {
        if( interactable is not IControllable controllable ) return;

        SwitchControlTo( controllable );
    }

    public void SwitchControlTo( IControllable controllable )
    {
        if( currentControllable != null )
        {
            currentControllable.EnableControl = false;
        }

        currentControllable = controllable;
        currentControllable.EnableControl = true;
        CameraHandler.CameraMode = currentControllable.CameraMode;
        
        if( currentControllable is not Node3D node3D|| currentControllable == Player ) return;

        playerRelativePosition = node3D.ToLocal( Player.GlobalPosition );
        playerRelativeRotation = Player.GlobalRotation - node3D.GlobalRotation;
    }
}
