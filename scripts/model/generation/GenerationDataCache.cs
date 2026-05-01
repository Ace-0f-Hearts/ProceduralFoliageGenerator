using System.IO;
using Godot;
using ProceduralFoliageGenerator.ViewModel;

namespace ProceduralFoliageGenerator.Model;

public class GenerationDataCache
{
    public static string CacheLocation { get; } = "user://GenerationCache";

    public static void SaveFiles(GenerationCommandData data)
    {
        var time = Time.GetDatetimeDictFromSystem();

        var cacheInstanceLocation = CacheLocation + "/" + time["year"] + "-" + time["month"] + "-" + time["day"] + "-" +
                                    time["hour"] + "-" + time["minute"] + "-";


        var dir = DirAccess.Open("user://");
        dir.MakeDir(CacheLocation);

        var newOcad = cacheInstanceLocation + Path.GetFileName(data.PathToMapFile);
        var newSymbolSet = cacheInstanceLocation + Path.GetFileName(data.PathToSymbolSet);
        var newSpeciesAttributes = cacheInstanceLocation + Path.GetFileName(data.PathToSpeciesAttributes);
        var newInstances = cacheInstanceLocation + Path.GetFileName(data.PathToPlantInstances);
        var newHeightMap = cacheInstanceLocation + Path.GetFileName(data.HeightMapOptions.Path);
        
        var error = dir.Copy(data.PathToMapFile, newOcad);
        if (error != Error.Ok)
            GD.PrintErr(error);
        
        error = dir.Copy(data.PathToSymbolSet, newSymbolSet);
        if (error != Error.Ok)
            GD.PrintErr(error);
        
        error = dir.Copy(data.PathToSpeciesAttributes, newSpeciesAttributes);
        if (error != Error.Ok)
            GD.PrintErr(error);
        
        error = dir.Copy(data.HeightMapOptions.Path, newHeightMap);
        if (error != Error.Ok)
            GD.PrintErr(error);

        if (data.DiffusionPointsOptions.Flag == DiffusionPointsAccusitionFlag.FromFile)
        {
            var newDiffusionPoints = cacheInstanceLocation + Path.GetFileName(data.DiffusionPointsOptions.Path);
            error = dir.Copy(data.DiffusionPointsOptions.Path, newDiffusionPoints);
            if (error != Error.Ok)
                GD.PrintErr(error);
            data.DiffusionPointsOptions.Path = ProjectSettings.GlobalizePath(newDiffusionPoints);
        }

        data.HeightMapOptions.Path = ProjectSettings.GlobalizePath(newHeightMap);
        data.PathToMapFile = ProjectSettings.GlobalizePath(newOcad);
        data.PathToSymbolSet = ProjectSettings.GlobalizePath(newSymbolSet);
        data.PathToSpeciesAttributes = ProjectSettings.GlobalizePath(newSpeciesAttributes);
        data.PathToPlantInstances = ProjectSettings.GlobalizePath(newInstances);
    }
}