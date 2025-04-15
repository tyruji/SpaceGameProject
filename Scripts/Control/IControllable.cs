public interface IControllable
{
    public bool EnableControl { get; set; }
    public ICameraMode CameraMode { get; }
}

/// <summary>
/// IControllable objects with an IPlayerUnexitable interface
/// cannot be exited directly by the player, and 
/// have to be exited manually in code.
/// </summary>
public interface IPlayerUnexitable {}