using Godot;

[GlobalClass, Tool]
public abstract partial class BlockLayerHandler : Resource
{
    [Export]
    public int PassIndex { get; set; } = 0;

    public abstract bool Handle( int x, int y, int z, int surface_level,
        VoxelToolMultipassGenerator voxelTool, int passIndex );
}