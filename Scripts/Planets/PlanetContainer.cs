using Godot;
using System;

public partial class PlanetContainer : Node3D
{
    public IPlanet[] Planets { get; private set; }

    public override void _Ready()
    {
        Planets = new IPlanet[ GetChildCount() ];
        
        for( int i = 0; i < Planets.Length; ++i )
        {
            Planets[ i ] = GetChild<IPlanet>( i );
        }
    }
}
