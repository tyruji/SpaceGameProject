using Godot;
using static CameraModes;

public class FreeCameraControllable : IControllable, IInteractable, IPlayerUnexitable
{
    public bool EnableControl { get; set; }

    public ICameraMode CameraMode => CAMERA_NONE_MODE;

    public bool InteractionEnabled { get; set; } = true;

    public void Interact( Node interactor ) {}
}