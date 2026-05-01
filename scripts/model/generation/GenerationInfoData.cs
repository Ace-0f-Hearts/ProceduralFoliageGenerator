using System;
using System.Collections.Generic;

namespace ProceduralFoliageGenerator.Model;

/// <summary>
///     Contains informational data that we are going to display to the user
/// </summary>
public record GenerationInfoData
{
    private List<PlantAttributes> _plantAttributes = new();
    private List<SymbolAttributes> _symbolAttributes = new();
    
    public List<PlantAttributes> PlantAttributes
    {
        get => _plantAttributes;
        set
        {
            _plantAttributes = value;
            InfoChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public List<SymbolAttributes> SymbolAttributes
    {
        get => _symbolAttributes;
        set
        {
            _symbolAttributes = value;
            InfoChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public int NumberOfRandomDiffusionPoints { get; set; } = 3;
    public int NumberOfPlantAttributes => PlantAttributes.Count;
    public int NumberOfSymbols => SymbolAttributes.Count;

    public event EventHandler InfoChanged;

    public bool IsWellFormed()
    {
        return PlantAttributes is not null && SymbolAttributes is not null && NumberOfPlantAttributes > 0 &&
               NumberOfSymbols > 0 && NumberOfRandomDiffusionPoints >= 0;
    }

    public void Clear()
    {
        PlantAttributes.Clear();
        SymbolAttributes.Clear();
        InfoChanged?.Invoke(this, EventArgs.Empty);

        NumberOfRandomDiffusionPoints = 3;
    }
}