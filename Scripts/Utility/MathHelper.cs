using Godot;

public static class MathHelper
{
    public static Vector3 FastRandomPointInUnitSphere()
    {
        Vector3 vec = new();
        vec.X = 2f * GD.Randf() -1f;
        vec.Y = 2f * GD.Randf() -1f;
        vec.Z = 2f * GD.Randf() -1f;
            // If vector is outside the sphere, divide by sqrt(3)
            // (the longest possible vector)
        if( vec.LengthSquared() > 1f ) vec *= .57735f;
        return vec;
    }
}