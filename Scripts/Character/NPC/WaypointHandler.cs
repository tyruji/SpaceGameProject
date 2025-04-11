using Godot;

public partial class WaypointHandler : Node
{
    [Export]
    public float MinimalDistanceToPoint { get; set; } = 1f;

    [Export]
    public Curve3D CurveToWalk
    {
        get => _curve;
        set
        {
            _curve = value;
            waypointType = eWaypointType.CURVE;
            _curveIndex = 0;
            CurrentWaypoint = _curve.GetPointPosition( _curveIndex );
        }
    }
    private Curve3D _curve = null;
    private int _curveIndex = 0;

    [Export]
    public Curve3D LoopingCurveToWalk
    {
        get => _curve;
        set
        {
            _curve = value;
            waypointType = eWaypointType.CURVE_LOOPING;
            _curveIndex = 0;
            CurrentWaypoint = _curve.GetPointPosition( _curveIndex );
        }
    }

    [Export]
    public Vector3 PointToWalk
    {
        get => _pointToWalk;
        set
        {
            CurrentWaypoint = _pointToWalk = value;
            waypointType = eWaypointType.POINT;
            _curveIndex = 0;
        }
    }
    private Vector3 _pointToWalk = Vector3.Zero;

    public Vector3 CurrentWaypoint { get; set; }

    public eWaypointType waypointType = eWaypointType.NONE;

    public Vector3 GetWalkDirection( Vector3 currentPosition )
    {
        return ( CurrentWaypoint - currentPosition ).Normalized();
    }

    public void UpdateWaypoint( Vector3 currentPosition )
    {
        switch( waypointType )
        {
            case eWaypointType.CURVE:
            HandleDefault( currentPosition );
            if( _curveIndex >= _curve.PointCount ) 
            {
                waypointType = eWaypointType.NONE;
                return;
            }
            CurrentWaypoint = _curve.GetPointPosition( _curveIndex );
            break;

            case eWaypointType.CURVE_LOOPING:
            HandleDefault( currentPosition );
            _curveIndex %= _curve.PointCount;
            CurrentWaypoint = _curve.GetPointPosition( _curveIndex );
            break;

            case eWaypointType.POINT:
            HandleDefault( currentPosition );
            if( _curveIndex > 0 ) 
            {
                waypointType = eWaypointType.NONE;
                return;
            }
            break;
        }
    }

    private void HandleDefault( Vector3 currentPos )
    {
        float min_dist_sqr = MinimalDistanceToPoint * MinimalDistanceToPoint;
        float dist_sqr = CurrentWaypoint.DistanceSquaredTo( currentPos );

        if( dist_sqr > min_dist_sqr ) return;

        ++_curveIndex;
    }

    public enum eWaypointType
    {
        CURVE,
        CURVE_LOOPING,
        POINT,
        NONE
    }
}