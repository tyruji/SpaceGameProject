using Godot;

public interface ICharacter
{
    public float Gravity { get; set; }

    public float MaxSpeed { get; set; }

    public float JumpSpeed { get; set; }

    public float Acceleration { get; set; }

    public float Friction { get; set; }

    public Vector3 Velocity { get; set; }

    public Vector3 MoveDirection { get; set; }

    public Basis GlobalBasis { get; set; }

    public bool WantsToJump { get; set; }
}