using System;
using Godot;
using ProceduralFoliageGenerator.Model;

namespace ProceduralFoliageGenerator.ViewModel;


public enum HeightMapAcquisitionFlag
{
    Random,
    Default,
    FromFile
}
public record HeightMapOptions
{
    private HeightMapAcquisitionFlag _flag;  
    private NoiseTexture2D _defaultNoiseTexture;
    private FastNoiseLite _defaultNoise;
    private String _defaultPath = "user://GenerationCache/heightmap.jpeg";
    
    
    public HeightMapAcquisitionFlag Flag
    {
        get => _flag;

        set
        {
            _flag = value;
            if (value == HeightMapAcquisitionFlag.Random)
                GenerateRandomHeightMap();
            
        }
    }


    
    public String Path { get; set; } = String.Empty;



    public HeightMapOptions()
    {
        Flag = HeightMapAcquisitionFlag.FromFile;
        
        {
            _defaultNoiseTexture = new NoiseTexture2D();
            _defaultNoise = new FastNoiseLite();
            var random = new RandomNumberGenerator();
            random.Randomize();
            _defaultNoise.SetSeed(random.RandiRange(int.MinValue, int.MaxValue));
            _defaultNoise.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;
            _defaultNoise.Frequency = 0.0004f;
            _defaultNoise.FractalOctaves = 5;
            
            _defaultNoiseTexture.SetNoise(_defaultNoise);
            _defaultNoiseTexture.Seamless = true;
            _defaultNoiseTexture.SetWidth(2048);
            _defaultNoiseTexture.SetHeight(2048);
            _defaultNoiseTexture.GenerateMipmaps = true;
        }

        
    }

    public HeightMapOptions(HeightMapAcquisitionFlag flag)
    {
        Flag = flag;
    }

    public async void GenerateRandomHeightMap()
    {
        var random = new RandomNumberGenerator();
        random.Randomize();
        _defaultNoise.SetSeed(random.RandiRange(int.MinValue, int.MaxValue));

        await _defaultNoiseTexture.ToSignal(_defaultNoiseTexture, NoiseTexture2D.SignalName.Changed);

        _defaultNoiseTexture.GetImage().SaveJpg(_defaultPath);
        Path = _defaultPath;
    }
    
    public bool Ready()
    {
        bool ready = false;
        ready = ready || (Flag == HeightMapAcquisitionFlag.Random);
        ready = ready || (Flag == HeightMapAcquisitionFlag.Default);
        ready = ready || (Flag == HeightMapAcquisitionFlag.FromFile && Path.Length > 0 &&  FileAccess.FileExists(Path) && (System.IO.Path.GetExtension(Path) == ".jpeg" ||System.IO.Path.GetExtension(Path) == ".jpg" || System.IO.Path.GetExtension(Path) == ".png") );

        return ready;
    }

    public void Clear()
    {
        Flag = HeightMapAcquisitionFlag.FromFile;
        Path = String.Empty;
    }
}