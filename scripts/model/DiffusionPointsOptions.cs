using System.Collections.Generic;
using Godot;

namespace ProceduralFoliageGenerator.ViewModel;

public enum DiffusionPointsAccusitionFlag
{
    Random,
    Manual,
    FromFile
}

public record DiffusionPointsOptions
{
    public DiffusionPointsAccusitionFlag Flag { get; set; } = DiffusionPointsAccusitionFlag.Random;
    public int NumberOfPoints { get; set; } = 3;
    public string Path { get; set; } = string.Empty;
    public List<DiffusionPoint> Points { get; set; } = new();

    public bool Ready()
    {
        var ready = false;
        ready = ready || (Flag == DiffusionPointsAccusitionFlag.Random && NumberOfPoints > 0);
        ready = ready || (Flag == DiffusionPointsAccusitionFlag.Manual && Points.Count > 0);
        ready = ready || (Flag == DiffusionPointsAccusitionFlag.FromFile && Path.Length > 0 &&
                          FileAccess.FileExists(Path) && System.IO.Path.GetExtension(Path) == ".json");

        return ready;
    }

    public void Clear()
    {
        Points.Clear();
        Path = string.Empty;
        NumberOfPoints = 3;
        Flag = DiffusionPointsAccusitionFlag.Random;
    }
}