using Godot;
using System;

public partial class ParkingBrakeButton : Area3D, IInteractable, IHighlightable
{
    [Export]
    public Node3D HighlightNode { get; set; }

    [Export]
    public Node3D ParkingBrakeOnLight { get; set; }

    public bool InteractionEnabled { get; set; } = true;

    public IFreezable Freezable { get; set; }

    public override void _Ready()
    {
        Freezable ??= GetParent<IFreezable>();
        ParkingBrakeOnLight ??= GetNode<Node3D>( nameof( ParkingBrakeOnLight ) );
    }

    public void Interact( Node interactor )
    {
        Freezable.Freeze = !Freezable.Freeze;
        ParkingBrakeOnLight.Visible = Freezable.Freeze;
    }

    public void Highlight()
    {
        HighlightNode?.Show();
    }

    public void Unhighlight()
    {
        HighlightNode?.Hide();
    }
}
