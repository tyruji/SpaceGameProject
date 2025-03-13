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

    public override void _ExitTree()
    {
        Player.OnInteract -= OnInteract;
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
    }
}
