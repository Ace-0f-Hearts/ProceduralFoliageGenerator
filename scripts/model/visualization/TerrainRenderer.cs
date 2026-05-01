using Godot;
using ProceduralFoliageGenerator.Model;

/// <summary>
///     Responsible node rendering the terrain using bump maps and dimensional information.
/// </summary>
public partial class TerrainRenderer : Node3D
{
    private Texture2D _bumpMap;
    private MapData _mapData;
    private Texture2D _normalMap;


    public MapData MapData
    {
        get => _mapData;
        set
        {
            _mapData = value;

            Terrain.SetPosition(new Vector3(MapData.Width / MapData.Scaling / 2.0f, 0,
                MapData.Height / MapData.Scaling / 2.0f));

            (Terrain.Mesh as QuadMesh)?.SetSize(new Vector2(MapData.Width / MapData.Scaling,
                MapData.Height / MapData.Scaling));
            (Terrain.Mesh as QuadMesh)?.SetSubdivideDepth(64);
            (Terrain.Mesh as QuadMesh)?.SetSubdivideWidth(64);
            (Terrain.MaterialOverride as ShaderMaterial)?.SetShaderParameter("height_scale", MapData.HeightScale);
            (Terrain.MaterialOverride as ShaderMaterial)?.SetShaderParameter("width_x",
                MapData.Width * 2 / MapData.Scaling);
            (Terrain.MaterialOverride as ShaderMaterial)?.SetShaderParameter("width_z",
                MapData.Height * 2 / MapData.Scaling);
        }
    }


    [Export] public MeshInstance3D Terrain { get; private set; }

    [Export] public FastNoiseLite DefaultNoise { get; private set; }
    [Export] public NoiseTexture2D DefaultNoiseTexture { get; private set; }
    [Export] public NoiseTexture2D DefaultNoiseNormal { get; private set; }


    [Export]
    public Texture2D BumpMap
    {
        get => _bumpMap;
        set
        {
            _bumpMap = value;
            (Terrain.MaterialOverride as ShaderMaterial)?.SetShaderParameter("bump_map", BumpMap);
        }
    }

    public Texture2D NormalMap
    {
        get => _normalMap;
        set
        {
            _normalMap = value;
            (Terrain.MaterialOverride as ShaderMaterial)?.SetShaderParameter("normal_map", NormalMap);
        }
    }

    public override void _Ready()
    {
        MapData = new MapData();

        GenerateRandomBumpMap();

        base._Ready();
    }

    public async void GenerateRandomBumpMap()
    {
        var random = new RandomNumberGenerator();
        random.Randomize();
        DefaultNoise.SetSeed(random.RandiRange(int.MinValue, int.MaxValue));

        DefaultNoiseTexture.SetNoise(DefaultNoise);
        DefaultNoiseNormal.SetNoise(DefaultNoise);
        await ToSignal(DefaultNoiseTexture, NoiseTexture2D.SignalName.Changed);
        await ToSignal(DefaultNoiseNormal, NoiseTexture2D.SignalName.Changed);

        BumpMap = DefaultNoiseTexture;
        NormalMap = DefaultNoiseNormal;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);
    }

    public void SetBumpMap(Image bumpMap)
    {
        BumpMap = ImageTexture.CreateFromImage(bumpMap);
    }

    public void SetMapTexture(Image mapTexture)
    {
        var tex = ImageTexture.CreateFromImage(mapTexture);
        (Terrain.MaterialOverride as ShaderMaterial)?.SetShaderParameter("tex", tex);
    }
}