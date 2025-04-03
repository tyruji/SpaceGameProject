using Godot;
using System;

[GlobalClass, Tool]
public partial class CustomVoxelWorldGeneratorCB : VoxelGeneratorMultipassCB
{
    // ---------------------------------------------------------------------------- \\
    //      For now this is in C#, but when Im done with the generation code,
    //      port it to C++.
    // ---------------------------------------------------------------------------- \\

    public const int VOXEL_BUFFER_CHANNEL = ( int ) VoxelBuffer.ChannelId.ChannelType;

    [Export]
    public BiomeLibrary BiomeLibrary { get; set;}

    public override int _GetUsedChannelsMask() => 1 << VOXEL_BUFFER_CHANNEL;

    public override void _GeneratePass( VoxelToolMultipassGenerator voxelTool, int passIndex )
    {
        var min_pos = voxelTool.GetMainAreaMin();
        var max_pos = voxelTool.GetMainAreaMax();

        voxelTool.Channel = VOXEL_BUFFER_CHANNEL;

        for( int z = min_pos.Z; z < max_pos.Z; ++z )
        {
            for( int x = min_pos.X; x < max_pos.X; ++x )
            {   
                var biome = BiomeLibrary.GetBiomeAt( x, z );
                float noise = BiomeLibrary.BiomeHeightNoise.GetNoise2D( x, z );
                biome.ProcessColumn( x, z, noise, voxelTool, passIndex );
            }
        }
    }
}
