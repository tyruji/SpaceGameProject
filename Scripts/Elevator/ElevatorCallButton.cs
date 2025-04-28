using Godot;
using System;

public partial class ElevatorCallButton : Area3D, IHighlightable, IInteractable
{
    [Export]
    public Elevator Elevator { get; set; }

    [Export]
    public Vector3 ElevatorPosition { get; set; }

    [Export]
    public MeshInstance3D OutlineMesh { get; set; }

    bool IInteractable.InteractionEnabled { get; set; } = true;

    public override void _Ready()
    {
        OutlineMesh ??= GetNode<MeshInstance3D>( nameof( OutlineMesh ) );
    }

    void IHighlightable.Highlight()
    {
        OutlineMesh.Show();
    }

    void IHighlightable.Unhighlight()
    {
        OutlineMesh.Hide();
    }

    void IInteractable.Interact( Node interactor )
    {
        Elevator?.CallElevator( ElevatorPosition );
    }
}
