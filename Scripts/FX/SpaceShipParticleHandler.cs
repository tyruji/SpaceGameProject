using Godot;
using System;

public partial class SpaceShipParticleHandler : Node3D
{
    [Export]
    public SpaceShip Ship { get; set; }

    [Export]
    public GpuParticles3D SmokeParticles { get; set; }

    public override void _Ready()
    {
        Ship ??= GetParent<SpaceShip>();
        SmokeParticles ??= GetNode<GpuParticles3D>( nameof( SmokeParticles ) );
    }

    public override void _Process( double delta )
    {
        if( Ship.EnableControl )
        {
            if( !SmokeParticles.Emitting ) SmokeParticles.Emitting = true;

            float new_amount_ratio = 1f;

            if( Ship.forwardInput == 0 ) new_amount_ratio = .2f;
        
            if( SmokeParticles.AmountRatio == new_amount_ratio ) return;
            SmokeParticles.AmountRatio = new_amount_ratio;
            return;
        }

        if( SmokeParticles.Emitting ) SmokeParticles.Emitting = false;
    }
}
