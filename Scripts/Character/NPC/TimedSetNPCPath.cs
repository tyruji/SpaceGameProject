using Godot;
using System;

public partial class TimedSetNPCPath : Path3D
{
    [Export]
    public NodePath NpcNodePath { get; set; }

    [Export]
    public float TimeToSetPath { get; set; } = 4f;

    private float _timeElapsed = 0f;

    public override void _Process( double delta )
    {
        _timeElapsed += ( float ) delta;

        if( _timeElapsed < TimeToSetPath ) return;

            // For now its MainRoboNpc, change to a more general interface later.
        ( GetNode( NpcNodePath ) as MainRoboNpc ).WaypointHandler.LoopingCurveToWalk = Curve;

        ProcessMode = ProcessModeEnum.Disabled;
    }
}
