using Godot;
using System;

public partial class InteractableProxy : Area3D, IInteractable, IHighlightable
{
    [Export]
    public NodePath ActualInteractableNodePath { get; set; }

    [Export]
    public Node3D HighlightNode { get; set; }

    [Export]
    public bool DisableOutsideInteractions { get; set; } = true;

    public bool InteractionEnabled { get; set; } = true;

    public override void _Ready()
    {
        ActualInteractableNodePath ??= GetParent().GetPath();
    }

    public void Interact( Node interactor )
    {
        if( interactor is not Player player ) return;

        Node interactableNode = GetNode( ActualInteractableNodePath );

        if( interactableNode is not IInteractable interactable ) return;

        if( DisableOutsideInteractions )
        {
            interactable.InteractionEnabled = true;
            player.NotifyInteraction( interactable );
            interactable.InteractionEnabled = false;
            
            return;
        }
        
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
