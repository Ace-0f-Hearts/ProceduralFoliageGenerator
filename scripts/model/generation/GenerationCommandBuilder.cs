using System;
using System.Collections.Generic;
using Godot;
using ProceduralFoliageGenerator.ViewModel;

namespace ProceduralFoliageGenerator.Model;

/// <summary>
/// Responsible for constructing the string containing the flags for the command to be executed
/// </summary>
public class GenerationCommandBuilder
{


    
    public static List<String> BuildHeightMapFlag(GenerationCommandData data)
    {
        List<String> resultFlags = new();
        switch (data.HeightMapOptions.Flag)
        {
            case HeightMapAcquisitionFlag.FromFile:
                resultFlags.Add("--height_map");
                resultFlags.Add(data.HeightMapOptions.Path);
                break;
            case HeightMapAcquisitionFlag.Random:
                resultFlags.Add("--height_map");
                resultFlags.Add(ProjectSettings.GlobalizePath(data.HeightMapOptions.Path));
                break;
            default:
                throw  new ArgumentException($"Invalid heightmap option {data.HeightMapOptions.Flag}");
        }
        return resultFlags;
    }

    public static List<String> BuildDiffusionPointsFlag(GenerationCommandData data)
    {
        List<String> resultFlags = new();
        switch (data.DiffusionPointsOptions.Flag)
        {
            case DiffusionPointsAccusitionFlag.Random:
                resultFlags.Add("--random_diff");
                resultFlags.Add(data.DiffusionPointsOptions.NumberOfPoints.ToString());
                break;
            case DiffusionPointsAccusitionFlag.Manual:
                break;
            case DiffusionPointsAccusitionFlag.FromFile:
                resultFlags.Add("--diff");
                resultFlags.Add(data.DiffusionPointsOptions.Path);
                break;
        }

        return resultFlags;
    }
    public static (string,string[]) Build(GenerationCommandData data)
    {
        var time = Time.GetDatetimeDictFromSystem();
        var location = GenerationDataCache.CacheLocation + "/" + time["year"] + "-" + time["month"] + "-" + time["day"] +  "-"  + time["hour"] + "-" + time["minute"] + "-";
        
        var configFileName = location + "config.json";
        var mapDataFile = location + "map_data.json";
        var mapTexture =  location + "map_texture.jpeg";
        
        configFileName = ProjectSettings.GlobalizePath(configFileName);
        mapDataFile = ProjectSettings.GlobalizePath(mapDataFile);
        mapTexture = ProjectSettings.GlobalizePath(mapTexture);
        var buildInstructions = new List<(string, Func<string>)>
        {
            ("--ocad",() => data.PathToMapFile),
            ("--species", () => data.PathToSpeciesAttributes),
            ("--symbol", () => data.PathToSymbolSet),
            ("--out", () => data.PathToPlantInstances),
            ("--config", () => { return configFileName; }),
            ("--write_map_data", () => {return mapDataFile; }),
            ("--foliage_img", () => { return mapTexture; }),
        };
        
        bool isReady = data.IsReady();
        List<String> command = new ();
        if (isReady) 
        {
            foreach (var (option,instruction) in buildInstructions)
            {
                command.Add(option);
                command.Add(instruction());
            }
            command.AddRange(BuildDiffusionPointsFlag(data));
            command.AddRange(BuildHeightMapFlag(data));
        }
        

        
        return (configFileName, command.ToArray());
    }
}