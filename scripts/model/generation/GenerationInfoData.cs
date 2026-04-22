using System;
using System.Collections.Generic;

namespace ProceduralFoliageGenerator.Model;

/// <summary>
/// Contains informational data that we are going to display to the user
/// </summary>
public record GenerationInfoData
{
    public List<PlantAttributes> PlantAttributes { get; set; } = new();
    public int NumberOfRandomDiffusionPoints { get; set; } = 3;
    public int GetAmountOfSpecies => PlantAttributes.Count;

    public event EventHandler InfoChanged;
    
    public void Clear()
    {
        PlantAttributes.Clear();
        NumberOfRandomDiffusionPoints = 3;
    }
}