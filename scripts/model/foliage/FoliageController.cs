using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using ProceduralFoliageGenerator.scripts.model;

namespace ProceduralFoliageGenerator.Model;

public class FoliageController
{
    
    public FoliageConfig Config { get; set; }
    public FoliageData Data { get; set; }
    
    public event EventHandler BuilderReady;
    
    public FoliageController()
    {
        Config =  new FoliageConfig();
        Data = new FoliageData();
    }

    public void ParseConfig(string path)
    {
        var content = File.ReadAllText(path);

        Config = FoliageConfigParser.Parse(content);
    }
    
    public void Populate()
    {
        String content;
        try
        {
            content = File.ReadAllText(Config.PathToSpeciesAttributes);
            Data.PlantAttributes = PlantAttributeParser.Parse(content);
        }
        catch (Exception e)
        {
            GD.Print(e);
            throw;
        }
        
        try
        {
            content =  File.ReadAllText(Config.PathToInstances);
            Data.PlantInstances = PlantInstancesParser.Parse(content);
        }
        catch (Exception e)
        {
            GD.Print(e);
            throw;
        }
        
        try
        {
            content =  File.ReadAllText(Config.PathToMapData);
            Data.MapData = MapDataParser.Parse(content);
        }
        catch (Exception e)
        {
            GD.Print(e);
            throw;
        }

        try
        {
            Data.HeightMap = Image.LoadFromFile(Config.PathToHeightMap);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        
        
        BuilderReady?.Invoke(this, EventArgs.Empty);
    }

    public Foliage Build()
    {
        var instancesPerSpecies = Data.GetPlantInstancesPerSpecies();

        List<SpeciesData> speciesData = new();
        foreach (var (attr,instances) in instancesPerSpecies)
        {
            speciesData.Add(BuildSpeciesData(attr, PlantObject.Default(), instances));
        }

        return new Foliage(speciesData);
    }

    private SpeciesData BuildSpeciesData(PlantAttributes attribute,PlantObject obj, List<PlantInstance> instances)
    {
        return new SpeciesData(attribute, obj, instances);
    }
}