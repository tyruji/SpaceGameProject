using Godot;

[GlobalClass, Tool]
public partial class BiomeGenerator : Resource
{
    [Export]
    public Color BiomeColor { get; set; } = Colors.Green;

    [Export]
    public string BiomeName { get; set; } = "Default Biome";

    [Export]
    public Zn_FastNoiseLite Noise { get; set; } = new Zn_FastNoiseLite();

    /// <summary>
    /// Chained layers, only one will get handled.
    /// </summary>
    [Export]
    public BlockLayerHandler[] BlockLayerChain { get; set; }

    /// <summary>
    /// Layers that will always be handled.
    /// </summary>
    [Export]
    public BlockLayerHandler[] ExtraBlockLayers { get; set; }

    public void ProcessColumn( int x, int z, float noise, VoxelToolMultipassGenerator voxelTool, int passIndex )
    {
        var min_pos = voxelTool.GetMainAreaMin();
        var max_pos = voxelTool.GetMainAreaMax();

        int surface_pos = voxelTool.GetSurfaceLevel( noise );

        for( int y = min_pos.Y; y < max_pos.Y; ++y )
        {
            foreach( var layer in BlockLayerChain )
            { 
                if( passIndex != layer.PassIndex ) continue;

                    // Search for the first layer that can handle this pass.
                if( layer.Handle( x, y, z, surface_pos, voxelTool, passIndex ) ) break;
            }
        }

        foreach( var layer in ExtraBlockLayers )
        { 
            if( passIndex != layer.PassIndex ) continue;
            
            layer.Handle( x, voxelTool.GetColumnCenterY(), z, surface_pos, voxelTool, passIndex );
        }
    }


    





        //VVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        //                     Probably to be deleted in the future.                     //
        //VVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        
    public void ProcessColumn( int x, int z, VoxelBuffer buffer,
                                Vector3I origin, int lod, uint channel )
    {
        int waterThreshold = 20;
        
        Vector3I column_world_pos = origin + new Vector3I( x << lod, 0, z << lod );
        float noiseVal = Noise.GetNoise2D( column_world_pos.X, column_world_pos.Z );
        int ground_pos = Mathf.RoundToInt( noiseVal * ( 100 + buffer.GetSize().Y ) ); // 100 is chunk height

        for( int y = 0; y < buffer.GetSize().Y; ++y )
        {
            Vector3I world_pos = column_world_pos;
            world_pos.Y += y;

            BlockType type = BlockType.GrassBlock;

            if( world_pos.Y > ground_pos )
            {
                if( world_pos.Y < waterThreshold )
                {
                    type = BlockType.Water;
                }
                else
                {
                    type = BlockType.Air;
                }
            }
            else if( world_pos.Y == ground_pos && y < waterThreshold )
            {
                type = BlockType.Sand;
            }
            else if( world_pos.Y == ground_pos )
            {
                type = BlockType.GrassBlock;
            }

            buffer.SetVoxel( ( ulong ) type, x, y, z, channel );
        }
    }

    // private ulong GetBlockId( Vector3I world_pos )
    // {
    //     float height = Noise.GetNoise2D( world_pos.X, world_pos.Z );

    //     ulong air_id = 0, grass_id = 1, sand_id = 2;

    //     ulong id = Select( grass_id, sand_id, height * .1f + height );
    //     return Select( id, air_id, height * 10f + world_pos.Y );
    // }

    // private ulong Select( ulong a, ulong b, float t ) => t < 0 ? a : b;
}