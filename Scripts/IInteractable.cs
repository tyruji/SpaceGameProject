using Godot;

public interface IInteractable
{
    bool InteractionEnabled { get; set; }

    void Interact( Node interactor );
}