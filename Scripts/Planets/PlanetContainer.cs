using Godot;
using System;

public partial class PlanetContainer : Node3D
{
    public Planet[] Planets { get; private set; }

    public override void _Ready()
    {
        Planets = new Planet[ GetChildCount() ];
        
        for( int i = 0; i < Planets.Length; ++i )
        {
            Planets[ i ] = GetChild<Planet>( i );
        }
    }
}
