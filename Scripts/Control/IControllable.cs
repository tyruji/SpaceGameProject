public interface IControllable
{
    public bool EnableControl { get; set; }
    public ICameraMode CameraMode { get; }
}