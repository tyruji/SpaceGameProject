using Godot;
using System;

public partial class AudioManager : Node3D
{
    public static AudioManager Instance { get; private set; }

    private const int AUDIO_STREAM_3D_COUNT = 16;
    private const int AUDIO_STREAM_COUNT = 16;

    private AudioStreamInfo[] _audioStreams = new AudioStreamInfo[ AUDIO_STREAM_COUNT ];
    private AudioStreamInfo3D[] _audio3DStreams = new AudioStreamInfo3D[ AUDIO_STREAM_3D_COUNT ];

    public override void _Ready()
    {
        Instance = this;

            // Setup audio players
        for( int i = 0; i < AUDIO_STREAM_COUNT; ++i )
        {
            AudioStreamPlayer plyr = new AudioStreamPlayer();
            AddChild( plyr );
            plyr.Owner = this;
            _audioStreams[ i ].audioPlayer = plyr;
        }
        for( int i = 0; i < AUDIO_STREAM_3D_COUNT; ++i )
        {
            AudioStreamPlayer3D plyr = new AudioStreamPlayer3D();
            AddChild( plyr );
            plyr.Owner = this;
            _audio3DStreams[ i ].audioPlayer = plyr;
        }
    }

    public void Play( AudioStream stream )
    {
        foreach( var stream_info in _audioStreams )
        {
            if( stream_info.IsBusy() ) continue;

            var plyr = stream_info.audioPlayer;
            plyr.Stream = stream;
            plyr.Play();
            break;
        }
    }

    public void Play( ISoundSettings soundSettings )
    {
        foreach( var stream_info in _audioStreams )
        {
            if( stream_info.IsBusy() ) continue;

            var plyr = stream_info.audioPlayer;
            plyr.Stream = soundSettings.Sound;
            plyr.PitchScale = soundSettings.Pitch;
            plyr.VolumeDb = soundSettings.Volume;
            plyr.Play();
            break;
        }
    }

    public void Play3D( AudioStream stream, Vector3 position, float unitSize = 1 )
    {
        foreach( var stream_info in _audio3DStreams )
        {
            if( stream_info.IsBusy() ) continue;

            var plyr = stream_info.audioPlayer;
            plyr.Stream = stream;
            plyr.Position = position;
            plyr.UnitSize = unitSize;
            plyr.Play();
            break;
        }
    }

    public void Play3D( ISoundSettings soundSettings, Vector3 position )
    {
        foreach( var stream_info in _audio3DStreams )
        {
            if( stream_info.IsBusy() ) continue;

            var plyr = stream_info.audioPlayer;
            plyr.Stream = soundSettings.Sound;
            plyr.Position = position;
            plyr.PitchScale = soundSettings.Pitch;
            plyr.VolumeDb = soundSettings.Volume;
            plyr.Play();
            break;
        }
    }

    public void Play3D( ISoundSettings3D soundSettings, Vector3 position )
    {
        foreach( var stream_info in _audio3DStreams )
        {
            if( stream_info.IsBusy() ) continue;

            var plyr = stream_info.audioPlayer;
            plyr.Stream = soundSettings.Sound;
            plyr.Position = position;
            plyr.PitchScale = soundSettings.Pitch;
            plyr.VolumeDb = soundSettings.Volume;
            plyr.UnitSize = soundSettings.UnitSize;
            plyr.Play();
            break;
        }
    }


    struct AudioStreamInfo
    {
        public AudioStreamPlayer audioPlayer;

        public bool IsBusy() => audioPlayer.Playing;
    }
    struct AudioStreamInfo3D
    {
        public AudioStreamPlayer3D audioPlayer;

        public bool IsBusy() => audioPlayer.Playing;
    }
}
