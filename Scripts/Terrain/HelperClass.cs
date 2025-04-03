using Godot;

public static class HelperClass
{
    public static int GetColumnSize( this VoxelToolMultipassGenerator voxelTool )
    {
        var min_pos = voxelTool.GetMainAreaMin();
        var max_pos = voxelTool.GetMainAreaMax();

        return Mathf.Abs( max_pos.Y - min_pos.Y );
    }

    public static int GetColumnCenterY( this VoxelToolMultipassGenerator voxelTool )
    {
        var min_pos = voxelTool.GetMainAreaMin();
        var max_pos = voxelTool.GetMainAreaMax();

        return ( max_pos.Y + min_pos.Y ) / 2;
    }

    public static int GetSurfaceLevel( this VoxelToolMultipassGenerator voxelTool, float noiseVal )
    {
        var min_pos = voxelTool.GetMainAreaMin();
        var max_pos = voxelTool.GetMainAreaMax();

        float average = min_pos.Y + max_pos.Y;
        average += voxelTool.GetColumnSize() * noiseVal;

        return Mathf.RoundToInt( .5f * average ); 
    }

    public static Basis GetSphereRotatedBasis( Vector3 up )
    {
        float angle = Mathf.Atan2( up.X, up.Z );

            // First rotate back to origin.
        up = up.Rotated( Vector3.Up, -angle );
        
        var forward_vec = new Vector3( 0, up.Z, -up.Y );

        var basis = new Basis( Vector3.Right, up, -forward_vec );

            // And rotate the basis back to desired angle.
        return basis.Rotated( Vector3.Up, angle );
    }
}