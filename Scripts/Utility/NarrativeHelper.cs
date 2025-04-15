using Godot;
using System;

public partial class NarrativeHelper : Node
{
    [Export]
    public Player Player { get; set; }

    [Export]
    public ControlFlowHandler ControlFlowHandler { get; set; }

    [Export]
    public CameraHandler CameraHandler { get; set; }

    public override void _Ready()
    {
        Player ??= GetParent().GetNode<Player>( nameof( Player ) );
        ControlFlowHandler ??= Player.GetNode<ControlFlowHandler>( nameof( ControlFlowHandler ) );
        CameraHandler ??= GetParent().GetNode<CameraHandler>( nameof( CameraHandler ) );
    }

    public void FreeCamera()
    {
            // Make the player switch control to the free camera controllable
        Player.NotifyInteraction( CameraHandler.FreeCameraControllable );
    }

    public void ReturnCameraToPlayer() => ControlFlowHandler.SwitchControlBackToPlayer();
}
