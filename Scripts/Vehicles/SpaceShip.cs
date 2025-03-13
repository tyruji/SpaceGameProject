using Godot;
using System;

public partial class SpaceShip : CharacterBody3D, IControllable, IInteractable
{
    [Export]
    public Node3D CameraPivot { get; set; }

    [Export]
    public SpringArm3D SpringArm { get; set; }

    [Export]
    public Node3D CameraFollowPoint { get; set; }

    public bool EnableControl { get; set; }

    public ICameraMode CameraMode
    {
        get
        {
            var mode = CameraModes.CAMERA_TIGHT_FOLLOW_MODE;
            mode.FollowTarget = CameraFollowPoint;
            return mode;
        }
    }

    public override void _Ready()
    {
        CameraPivot ??= GetNode<Node3D>( nameof( CameraPivot ) );
        SpringArm ??= CameraPivot.GetNode<SpringArm3D>( nameof( SpringArm3D ) );
        CameraFollowPoint ??= SpringArm.GetNode<Node3D>( nameof( CameraFollowPoint ) ); 
    }

    public void Interact( Node interactor ) {}
}
