using Godot;

[GlobalClass, Tool]
public partial class SurfaceLayerHandler : BlockLayerHandler
{
    [Export]
    public BlockType SurfaceBlockType { get; set; }

    public override bool Handle(int x, int y, int z, int surface_level, VoxelToolMultipassGenerator voxelTool, int passIndex)
    {
        if( y == surface_level )
        {
            voxelTool.SetVoxel( new Vector3I( x, y, z ), ( ulong ) SurfaceBlockType );
            return true;
        }
        return false;
    }
}