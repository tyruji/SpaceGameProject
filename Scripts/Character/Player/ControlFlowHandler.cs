using Godot;
using System;

public partial class ControlFlowHandler : Node
{
    [Export]
    public Player Player { get; set; }

    [Export]
    public CameraHandler CameraHandler { get; set;}

    public IControllable currentControllable = null;

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

            // Now transform the player's transform back to world
            // from the calculated local transform (we calculated it beforehand).
        Player.Transform = node3D.Transform * Player.Transform;
        SwitchControlTo( Player );
    }

    private void OnInteract( IInteractable interactable )
    {
        if( interactable is not IControllable controllable
            || !interactable.InteractionEnabled ) return;

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

            // Transform player's transform as a local to the node
            // to keep relative position and rotation to it.
        Player.Transform = node3D.Transform.AffineInverse() * Player.Transform;
    }
}
