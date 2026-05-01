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


        var error = dir.Copy(data.PathToMapFile, newOcad);
        error = dir.Copy(data.PathToSymbolSet, newSymbolSet);
        error = dir.Copy(data.PathToSpeciesAttributes, newSpeciesAttributes);

        if (data.HeightMapOptions.Flag == HeightMapAcquisitionFlag.FromFile)
        {
            var newHeightMap = cacheInstanceLocation + Path.GetFileName(data.HeightMapOptions.Path);
            dir.Copy(data.HeightMapOptions.Path, newHeightMap);
            data.HeightMapOptions.Path = ProjectSettings.GlobalizePath(newHeightMap);
        }

        if (data.DiffusionPointsOptions.Flag == DiffusionPointsAccusitionFlag.FromFile)
        {
            var newDiffusionPoints = cacheInstanceLocation + Path.GetFileName(data.DiffusionPointsOptions.Path);
            dir.Copy(data.DiffusionPointsOptions.Path, newDiffusionPoints);
            data.DiffusionPointsOptions.Path = ProjectSettings.GlobalizePath(newDiffusionPoints);
        }


        data.PathToMapFile = ProjectSettings.GlobalizePath(newOcad);
        data.PathToSymbolSet = ProjectSettings.GlobalizePath(newSymbolSet);
        data.PathToSpeciesAttributes = ProjectSettings.GlobalizePath(newSpeciesAttributes);
        data.PathToPlantInstances = ProjectSettings.GlobalizePath(newInstances);


        GD.Print(data.ToString());
    }
}