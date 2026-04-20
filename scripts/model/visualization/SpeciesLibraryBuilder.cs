using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;

namespace ProceduralFoliageGenerator.Model;

public class SpeciesLibraryBuilder
{
    public event EventHandler PlantInstancesSet;
    public event EventHandler PlantAttributesSet;
    public event EventHandler BuilderReady;
    
    private List<PlantAttributes> _plantAttributes;
    private List<PlantInstance> _plantInstances;
    
    // private Dictionary<PlantAttributes,List<PlantInstance>> _plantInstancesPerSpecies;
    
    
    public string PathToSpeciesAttributes { get; set; }

    
    public SpeciesLibraryBuilder()
    {
        _plantInstances = new();
        _plantAttributes = new();
    }

    public List<(string, int)> GetNumberOfInstancesPerSpecies => GetPlantInstancesPerSpecies().Select(i => (i.Key.Name,i.Value.Count)).ToList();

    public List<PlantInstance> PlantInstances
    {
        get  => _plantInstances;
        set
        {
            _plantInstances = value;
            PlantInstancesSet?.Invoke(this, EventArgs.Empty);
        }
    }
    public List<PlantAttributes> PlantAttributes
    {
        get => _plantAttributes;
        set
        {
            _plantAttributes = value;
            PlantAttributesSet?.Invoke(this, EventArgs.Empty);
        }
    }

    public SpeciesLibrary Build()
    {
        var instancesPerSpecies = GetPlantInstancesPerSpecies();

        List<SpeciesData> speciesData = new();
        foreach (var (attr,instances) in instancesPerSpecies)
        {
            speciesData.Add(BuildSpeciesData(attr, PlantObject.Default(), instances));
        }

        return new SpeciesLibrary(speciesData);
    }

    private SpeciesData BuildSpeciesData(PlantAttributes attribute,PlantObject obj, List<PlantInstance> instances)
    {
        return new SpeciesData(attribute, obj, instances);
    }
    
    public Dictionary<PlantAttributes, List<PlantInstance>> GetPlantInstancesPerSpecies()
    {
        Dictionary<PlantAttributes, List<PlantInstance>> result = new Dictionary<PlantAttributes, List<PlantInstance>>();

        foreach (var attr in PlantAttributes)
        {
            var instances = PlantInstances.FindAll((instance) => { return instance.Id ==  attr.Id; });
            result.Add(attr, instances);
        }
        
        return result;
    }

    
    
    public void Clear()
    {
        _plantInstances.Clear();
        _plantAttributes.Clear();
    }
}