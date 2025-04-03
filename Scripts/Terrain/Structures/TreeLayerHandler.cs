using Godot;

[GlobalClass, Tool]
public partial class TreeLayerHandler : BlockLayerHandler
{
    [Export]
    public TreeData TreeData { get; set;}

    [Export( PropertyHint.Range, "0.0, 1.0" )]
    public float TreeThreshold { get; set; } = .5f;

    [Export]
    public Zn_FastNoiseLite Noise { get; set; }

    public override bool Handle(int x, int y, int z,
        int surface_level, VoxelToolMultipassGenerator voxelTool, int passIndex)
    {
        if( voxelTool.GetVoxel( new Vector3I( x, surface_level, z ) ) != ( ulong ) BlockType.GrassBlock )
            return false;

        if( Noise.GetNoise2D( x, z ) < TreeThreshold ) return false;

            // Place tree
        Vector3I origin = new Vector3I( x, surface_level + 1, z );

        foreach( var wood_pos in TreeData.WoodPositions )
        {
            voxelTool.SetVoxel( origin + wood_pos, ( ulong ) BlockType.Wood );
        }
        foreach( var leaf_pos in TreeData.LeafPositions )
        {
            voxelTool.SetVoxel( origin + leaf_pos, ( ulong ) BlockType.Leaves );
        }

        return true;
    }
}