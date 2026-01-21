using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProceduralFoliageGenerator.Model;

/// <summary>
/// Structure for storing all foliage rendering and generation related data.
/// </summary>
public record DataStorage
{
    public event EventHandler PlantAttributesSet;
    public event EventHandler PlantInstancesSet;

    private List<PlantAttributes> _plantAttrs;
    private Dictionary<string,List<PlantInstanceDescriptor>> _plantInstances;
    
    public string PathToMapFile{ get; set;}
    public string PathToPlantFile { get; set; }
    
    public string PathToFoliageFile { get; set; }
    
    public List<PlantAttributes> PlantAttrs
    {
        get => _plantAttrs;
        set
        {
            _plantAttrs = value;
            PlantAttributesSet?.Invoke(this, EventArgs.Empty);
        }
    }

    public Dictionary<string, List<PlantInstanceDescriptor>> PlantInstances
    {
        get => _plantInstances;
        set
        {
            _plantInstances = value;
            PlantInstancesSet?.Invoke(this, EventArgs.Empty);
        }
    }

    public int GetAmountOfSpecies => PlantAttrs.Count;
    public List<(string, int)> GetPlantInstanceAmountPerSpecies => _plantInstances.Select(i => (i.Key,i.Value.Count)).ToList();

    public void Clear()
    {
        PlantAttrs.Clear();
        PlantInstances.Clear();
        PathToMapFile = String.Empty;
        PathToPlantFile = String.Empty;
        PathToFoliageFile = String.Empty;
    }
}