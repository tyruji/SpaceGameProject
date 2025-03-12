using Godot;
using System;

public partial class InteractableProxy : Area3D, IInteractable, IHighlightable
{
    [Export]
    public NodePath ActualInteractableNodePath { get; set; }

    [Export]
    public Node3D HighlightNode { get; set; }

    public void Interact( Node interactor )
    {
        if( interactor is not Player player ) return;

        Node interactableNode = GetNode( ActualInteractableNodePath );

        if( interactableNode is not IInteractable interactable ) return;

        player.NotifyInteraction( interactable );
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
