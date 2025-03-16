using Godot;

public interface IOrientationSpace
{
    Transform3D GlobalTransform { get; set; }
    Transform3D Transform { get; set;}
}