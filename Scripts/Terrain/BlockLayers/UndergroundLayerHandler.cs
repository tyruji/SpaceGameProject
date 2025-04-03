using Godot;

[GlobalClass, Tool]
public partial class UndergroundLayerHandler : BlockLayerHandler
{
    [Export]
    public BlockType UndergroundBlockType { get; set; }

    public override bool Handle(int x, int y, int z, int surface_level, VoxelToolMultipassGenerator voxelTool, int passIndex)
    {
        if( y < surface_level )
        {
            voxelTool.SetVoxel( new Vector3I( x, y, z ), ( ulong ) UndergroundBlockType );
            return true;
        }
        return false;
    }
}