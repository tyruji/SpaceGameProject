using Godot;

[GlobalClass, Tool]
public partial class AirLayerHandler : BlockLayerHandler
{
    public override bool Handle(int x, int y, int z, int surface_level, VoxelToolMultipassGenerator voxelTool, int passIndex)
    {
        if( y > surface_level )
        {
            voxelTool.SetVoxel( new Vector3I( x, y, z ), ( ulong ) BlockType.Air );
            return true;
        }
        return false;
    }
}