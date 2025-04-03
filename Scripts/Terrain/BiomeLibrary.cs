using Godot;

[GlobalClass, Tool]
public partial class BiomeLibrary : Resource
{
    [Export]
    public BiomeGenerator[] BiomeGenerators { get; set; }

    [Export]
    public Zn_FastNoiseLite BiomeHeightNoise { get; set; } = new Zn_FastNoiseLite();

    [Export]
    public Zn_FastNoiseLite BiomeHorizontalSelectionNoise { get; set; } = new Zn_FastNoiseLite();

    [Export]
    public Zn_FastNoiseLite BiomeVerticalSelectionNoise { get; set; } = new Zn_FastNoiseLite();

    [Export]
    public Texture2D BiomeLookupTexture { get; set;}

    [Export]
    private bool _GenerateImage
    {
        get => false;
        set 
        {
            if( BiomeLookupTexture == null ) return;

            var img = Image.CreateEmpty( 512, 512, false, Image.Format.Rgba8 );

            var biome_img = BiomeLookupTexture.GetImage();
            biome_img.Decompress();

            float water_level = .5f;

            lock( img.GetData() )
            {
                for( int j = 0; j < img.GetHeight(); ++j ) 
                {
                    for( int i = 0; i < img.GetWidth(); ++i )
                    {
                        // var horizontal = BiomeHorizontalSelectionNoise.GetNoise2D( i, j );
                        // var vertical = BiomeVerticalSelectionNoise.GetNoise2D( i, j ); 

                        // horizontal = Mathf.Clamp( horizontal, 0, 1 );
                        // vertical = Mathf.Clamp( vertical, 0, 1 );

                        // float w = biome_img.GetWidth() - 1;
                        // float h = biome_img.GetHeight() - 1; 
                        // int x = ( int ) ( horizontal * w );
                        // int y = ( int ) ( vertical * h );
                    
                        // var color = biome_img.GetPixel( x, y );
                    
                        float height = .5f * ( 1.0f + BiomeHeightNoise.GetNoise2D( i, j ) );
                        float dist = .5f * ( 1.0f + BiomeVerticalSelectionNoise.GetNoise2D( i, j ) );
                        float mix = .45f;
                        height = Mathf.Lerp( height, 1.0f - dist, mix );

                        var color = height < water_level ? ( height ) * Colors.Blue
                            : height * Colors.Green;

                        if( height > water_level && height < water_level + .01 )
                            color = (.25f + height) * Colors.Yellow;

                        if( height > water_level + .2f )
                            color = height * Colors.DarkGreen;

                        if( height > water_level + .25f )
                            color = height * Colors.Gray;

                        color.A = 1.0f;
                        img.SetPixel( i, j, color );
                    } 
                }
            }

            GD.Print( "Generated preview biome texture" );
            img.SavePng( "res://BiomeImage.png" );
            ResourceSaver.Save( img, "res://BiomeImage.png" );
        }
    }

    private int GetTerritoryNumber( float noise )
    {
        return ( int ) ( noise * 100 );
    }

    private BiomeGenerator[,] _BiomeIndexLookupTable
    {
        get
        {
            if( _biomeIndexLookupTableVar == null ) InitializeBiomeIndexTable();
            return _biomeIndexLookupTableVar;
        }
        set => _biomeIndexLookupTableVar = value;
    }

    private BiomeGenerator[,] _biomeIndexLookupTableVar = null;

    private void InitializeBiomeIndexTable()
    {
        var biome_img = BiomeLookupTexture.GetImage();
        if( biome_img.IsCompressed() ) biome_img.Decompress();

        var w = biome_img.GetWidth();
        var h = biome_img.GetHeight();
        _BiomeIndexLookupTable = new BiomeGenerator[ w, h ];
    
        for( int x = 0; x < w; ++x )
        {
            for( int y = 0; y < h; ++y )
            {
                var color = biome_img.GetPixel( x, y );
                _BiomeIndexLookupTable[ x, y ] = BiomeGenerators[ 0 ];
                foreach( var biome in BiomeGenerators )
                {
                    if( !biome.BiomeColor.IsEqualApprox( color ) ) continue;
                    _BiomeIndexLookupTable[ x, y ] = biome;
                    break;
                }
            }
        }
    }

    public BiomeGenerator GetBiomeAt( int x, int z )
    {
        return BiomeGenerators[ 0 ];
        var horizontal = BiomeHorizontalSelectionNoise.GetNoise2D( x, z );
        var vertical = BiomeVerticalSelectionNoise.GetNoise2D( x, z ); 

        horizontal = Mathf.Clamp( horizontal, 0, 1 );
        vertical = Mathf.Clamp( vertical, 0, 1 );

        float w = _BiomeIndexLookupTable.GetLength( 0 ) - 1;
        float h = _BiomeIndexLookupTable.GetLength( 1 ) - 1; 
        int i = ( int ) ( horizontal * w );
        int j = ( int ) ( vertical * h );

        return _BiomeIndexLookupTable[ i, j ];
    }
}