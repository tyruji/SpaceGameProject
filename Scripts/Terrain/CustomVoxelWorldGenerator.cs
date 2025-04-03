using Godot;
using System;

[GlobalClass, Tool]
public partial class CustomVoxelWorldGenerator : VoxelGeneratorScript
{
    // ---------------------------------------------------------------------------- \\
    //      For now this is in C#, but when Im done with the generation code,
    //      port it to C++.
    // ---------------------------------------------------------------------------- \\

    public const int VOXEL_BUFFER_CHANNEL = ( int ) VoxelBuffer.ChannelId.ChannelType;


    public BiomeGenerator biomeGenerator = new BiomeGenerator();


    public override int _GetUsedChannelsMask() => 1 << VOXEL_BUFFER_CHANNEL;

        // Main world generation method.
    public override void _GenerateBlock( VoxelBuffer buffer, Vector3I origin, int lod )
    {
        if( lod != 0 ) return;

        for( int z = 0; z < buffer.GetSize().Z; ++z )
        {
            for( int x = 0; x < buffer.GetSize().X; ++x )
            {
                biomeGenerator.ProcessColumn( x, z, buffer, origin, lod, VOXEL_BUFFER_CHANNEL );
            }
        }

        buffer.CompressUniformChannels();
    }
}
