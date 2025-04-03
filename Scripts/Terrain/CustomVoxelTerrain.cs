using Godot;
using System;

public partial class CustomVoxelTerrain : VoxelTerrain
{
    [Export]
    private string _DatabasePath { get; set; } = "";//@"res://Resources/Terrain/";

    // [Export]
    // private Node3D _FollowNode = null;

    // [Export]
    // private float _PlanetRadius = 50.0f;

    public override void _Ready()
    {
        VoxelStreamSQLite sql_stream = new VoxelStreamSQLite();
        sql_stream.DatabasePath = _DatabasePath + "WorldDatabase";
        Stream = sql_stream;

        //Generator = new CustomVoxelWorldGenerator();
    }

    // public override void _Process( double delta )
    // {
    //     base._Process( delta );

    //     if( _FollowNode == null ) return;

    //     Vector3 planet_center = Vector3.Zero;
    //     Vector3 up = ( _FollowNode.GlobalPosition - planet_center ).Normalized();


    //     float angle = Mathf.Atan2( up.X, up.Z );

    //     if( up.Z < 0 ) angle += Mathf.Pi;

    //         // First rotate back to origin.
    //     up = up.Rotated( Vector3.Up, -angle );

    //     var back_vec = new Vector3( 0, -up.Z, up.Y );
    //     var basis = new Basis( Vector3.Right, up, back_vec );

    //     var tr = Transform;
    //         // And rotate the basis back to desired angle.
    //     tr.Basis = basis;//basis.Rotated( Vector3.Up, angle );
    //     Transform = tr;

    //     float elevation_angle = Mathf.Acos( up.Y );
    //     if( up.Z < 0 ) elevation_angle = 2.0f * Mathf.Pi - elevation_angle;

    //     GlobalPosition = planet_center + _PlanetRadius * ( up - back_vec * elevation_angle );
    // }

    // public Basis GetSphereRotatedBasis( Vector3 up )
    // {
    //     float angle = Mathf.Atan2( up.X, up.Z );

    //         // First rotate back to origin.
    //     up = up.Rotated( Vector3.Up, -angle );

    //     var height = _PlanetRadius / up.Y;

    //     var forward_vec = new Vector3( 0, up.Z, -up.Y );

    //     var basis = new Basis( Vector3.Right, up, -forward_vec );

    //         // And rotate the basis back to desired angle.
    //     return basis.Rotated( Vector3.Up, angle );
    // }

    public override void _ExitTree()
    {
        SaveModifiedBlocks();
    }
}
