using Godot;

[GlobalClass, Tool]
public partial class WaterLayerHandler : BlockLayerHandler
{
    [Export]
    public int WaterLevel { get; set; } = 1;

    [Export]
    public int MaxSandReach { get; set; } = 3;

    [Export]
    public Zn_FastNoiseLite SandNoise { get; set; }

    public override bool Handle(int x, int y, int z, int surface_level, VoxelToolMultipassGenerator voxelTool, int passIndex)
    {
        if( y > surface_level && y <= WaterLevel )
        {
            voxelTool.SetVoxel( new Vector3I( x, y, z ), ( ulong ) BlockType.Water );

            if( y == surface_level + 1 )
            {
                voxelTool.SetVoxel( new Vector3I( x, surface_level, z ), ( ulong ) BlockType.Sand );
            }

            return true;
        }

        float sandNoise = SandNoise.GetNoise2D( x, z );
        if( y == surface_level && y >= WaterLevel
            && y < WaterLevel + ( 1.0f - sandNoise ) * MaxSandReach )
        {
            voxelTool.SetVoxel( new Vector3I( x, y, z ), ( ulong ) BlockType.Sand );
            return true;
        }
        return false;
    }
}