using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using ProceduralFoliageGenerator.scripts.model;

namespace ProceduralFoliageGenerator.Model;

public class FoliageController
{
    public FoliageController()
    {
        Config = new FoliageConfig();
        Data = new FoliageData();
    }

    public FoliageConfig Config { get; set; }
    public FoliageData Data { get; set; }
    public List<PlantObject> PlantObjects { get; set; }

    public event EventHandler BuilderReady;

    public event EventHandler<string> ErrorOccured;

    public void ParseConfig(string path)
    {
        Clear();
        var content = File.ReadAllText(path);

        Config = FoliageConfigParser.Parse(content);
    }

    public void Populate()
    {
        var problemWithConfig = false;

        if (!Config.IsReady())
        {
            ErrorOccured?.Invoke(this, "One or more necessary generation artifacts are unavailable!");
            return;
        }

        string content;
        try
        {
            content = File.ReadAllText(Config.PathToSpeciesAttributes);
            var list = PlantAttributeParser.Parse(content);
            if (list.Count > 0)
                Data.PlantAttributes = list;
            else
                problemWithConfig = true;
        }
        catch (Exception e)
        {
            ErrorOccured?.Invoke(this, "Error occured while reading file containing species attributes: " + e.Message);
            GD.Print(e);
            problemWithConfig = true;
        }

        try
        {
            content = File.ReadAllText(Config.PathToInstances);
            var list = PlantInstancesParser.Parse(content);

            if (list.Count > 0)
                Data.PlantInstances = list;
            else
                problemWithConfig = true;
        }
        catch (Exception e)
        {
            ErrorOccured?.Invoke(this, "Error occured while reading file containing instances: " + e.Message);
            GD.Print(e);
            problemWithConfig = true;
        }

        try
        {
            content = File.ReadAllText(Config.PathToMapData);
            var data = MapDataParser.Parse(content);
            if (data is not null)
                Data.MapData = data;
            else
                problemWithConfig = true;
        }
        catch (Exception e)
        {
            ErrorOccured?.Invoke(this, "Error occured while reading file containing map information: " + e.Message);
            GD.Print(e);
            problemWithConfig = true;
        }

        try
        {
            Data.HeightMap = Image.LoadFromFile(Config.PathToHeightMap);
        }
        catch (Exception e)
        {
            ErrorOccured?.Invoke(this, "Error occured while reading file containing height map: " + e.Message);
            GD.Print(e);
            problemWithConfig = true;
        }

        try
        {
            Data.MapTexture = Image.LoadFromFile(Config.PathToMapTexture);
        }
        catch (Exception e)
        {
            ErrorOccured?.Invoke(this, "Error occured while reading file containing map texture: " + e.Message);
            GD.Print(e);
            problemWithConfig = true;
        }

        if (problemWithConfig)
            return;

        BuilderReady?.Invoke(this, EventArgs.Empty);
    }

    public Foliage Build()
    {
        var instancesPerSpecies = Data.GetPlantInstancesPerSpecies();

        List<SpeciesData> speciesData = new();

        var idx = 0;
        foreach (var (attr, instances) in instancesPerSpecies)
        {
            speciesData.Add(BuildSpeciesData(attr, PlantObjects[idx % PlantObjects.Count], instances));
            ++idx;
        }

        return new Foliage(speciesData);
    }

    private SpeciesData BuildSpeciesData(PlantAttributes attribute, PlantObject obj, List<PlantInstance> instances)
    {
        return new SpeciesData(attribute, obj, instances);
    }

    public void Clear()
    {
        Config.Clear();
        Data.Clear();
    }
}