using Godot;

[GlobalClass, Tool]
public partial class StoneLayerHandler : BlockLayerHandler
{
    [Export( PropertyHint.Range, "0.0, 1.0" )]
    public float StoneThreshold { get; set; } = .5f;

    [Export]
    public Zn_FastNoiseLite Noise { get; set; }

    public override bool Handle(int x, int y, int z, int surface_level, VoxelToolMultipassGenerator voxelTool, int passIndex)
    {
        //if( y > surface_level ) return false;
        if( passIndex != 0 ) return false;

        var min_pos = voxelTool.GetMainAreaMin();
        var max_pos = voxelTool.GetMainAreaMax();

        for( y = min_pos.Y; y <= surface_level; ++y )
        {
            float noise = Noise.GetNoise2D( x, z );
            if( noise <= StoneThreshold ) continue;

            voxelTool.SetVoxel( new Vector3I( x, y, z ), ( ulong ) BlockType.Stone );
        }

        return true;
    }
}