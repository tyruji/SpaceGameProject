using Godot;

[GlobalClass, Tool]
public partial class GrassLayerHandler : BlockLayerHandler
{
    [Export( PropertyHint.Range, "0.0, 1.0" )]
    public float GrassThreshold { get; set; } = .5f;

    [Export]
    public Zn_FastNoiseLite Noise { get; set; }

    public override bool Handle(int x, int y, int z,
        int surface_level, VoxelToolMultipassGenerator voxelTool, int passIndex)
    {
        if( voxelTool.GetVoxel( new Vector3I( x, surface_level, z ) ) != ( ulong ) BlockType.GrassBlock )
            return false;

        if( voxelTool.GetVoxel( new Vector3I( x, surface_level + 1, z ) ) != ( ulong ) BlockType.Air )
            return false;

        if( Noise.GetNoise2D( x, z ) < GrassThreshold ) return false;

            // Place grass
        Vector3I origin = new Vector3I( x, surface_level + 1, z );
        voxelTool.SetVoxel( origin, ( ulong ) BlockType.Grass );

        return true;
    }
}