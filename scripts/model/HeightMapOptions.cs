using System;
using Godot;

namespace ProceduralFoliageGenerator.ViewModel;


public enum HeightMapAcquisitionFlag
{
    Random,
    Default,
    FromFile
}
public record HeightMapOptions
{
    public HeightMapAcquisitionFlag Flag { get; set; } =  HeightMapAcquisitionFlag.FromFile;
    public String Path { get; set; } = String.Empty;

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