using Godot;
using Godot.Collections;

[GlobalClass, Tool]
public partial class TreeData : Resource
{
    [Export]
    public Array<Vector3I> WoodPositions { get; set; }

    [Export]
    public Array<Vector3I> LeafPositions { get; set; }

}