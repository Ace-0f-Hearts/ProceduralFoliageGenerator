using System;
using System.Collections.Generic;
using System.IO;
using ProceduralFoliageGenerator.ViewModel;
using FileAccess = Godot.FileAccess;

namespace ProceduralFoliageGenerator.Model;

/// <summary>
///     Contains data that we will directly use for requesting the foliage generation
/// </summary>
public class GenerationCommandData
{
    private string _pathToSpeciesAttributes = string.Empty;
    private string _pathToSymbolSet = string.Empty;

    public string PathToSymbolSet
    {
        get => _pathToSymbolSet;
        set
        {
            _pathToSymbolSet = value;
            PathToSymbolSetChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public string PathToMapFile { get; set; } = string.Empty;

    public string PathToSpeciesAttributes
    {
        get => _pathToSpeciesAttributes;
        set
        {
            _pathToSpeciesAttributes = value;
            PathToSpeciesAttributesChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public string PathToPlantInstances { get; set; } = string.Empty;

    public DiffusionPointsOptions DiffusionPointsOptions { get; set; }
    public HeightMapOptions HeightMapOptions { get; set; }

    public event EventHandler PathToSymbolSetChanged;
    public event EventHandler PathToSpeciesAttributesChanged;

    public bool IsReady()
    {
        var predicates =
            new List<(Func<bool>, string)>
            {
                (() => { return FileAccess.FileExists(PathToSymbolSet); },
                    "Issue with file containing the symbol set: File does not exist\n"),
                (() => { return Path.HasExtension(PathToSymbolSet) && Path.GetExtension(PathToSymbolSet) == ".json"; },
                    "Issue with file containing the symbol set: Extension is missing or not supported. Supported extensions: '.json'\n"),
                (() => { return FileAccess.FileExists(PathToMapFile); },
                    "Issue with OCAD map file: File does not exist\n"),
                (() => { return Path.HasExtension(PathToMapFile) && (Path.GetExtension(PathToMapFile) == ".omap" || Path.GetExtension(PathToMapFile) == ".ocd"); },
                    "Issue with OCAD map file: Extension is missing or not supported. Supported extensions: '.ocad', '.ocd'\n"),
                (() => { return FileAccess.FileExists(PathToSpeciesAttributes); },
                    "Issue with file containing species attributes: File does not exist\n"),
                (() => { return Path.HasExtension(PathToSpeciesAttributes) && Path.GetExtension(PathToSpeciesAttributes) == ".json"; },
                    "Issue with file containing species attributes: Extension is missing or not supported. Supported extensions: '.json'\n"),
                (() => { return HeightMapOptions.Ready(); }, "Issue with height map: Invalid configuration\n"),
                (() => { return DiffusionPointsOptions.Ready(); }, "Issue with diffusion points: Invalid configuration")
            };
        var isReady = true;

        List<string> errors = new();

        foreach (var (predicate, errorMessage) in predicates)
            if (!predicate())
            {
                isReady = false;
                errors.Add(errorMessage);
            }

        return isReady;
    }

    public void Clear()
    {
        PathToSymbolSet = string.Empty;
        PathToMapFile = string.Empty;
        PathToSpeciesAttributes = string.Empty;
        PathToPlantInstances = string.Empty;
    }

    public override string ToString()
    {
        var res = string.Empty;

        res += "OCAD map: " + PathToMapFile + "\n";
        res += "Species attributes file: " + PathToSpeciesAttributes + "\n";
        res += "Symbol set: " + PathToSymbolSet + "\n";
        res += "Plant instance output: " + PathToPlantInstances + "\n";

        switch (HeightMapOptions.Flag)
        {
            case HeightMapAcquisitionFlag.FromFile:
                res += "Height map: " + HeightMapOptions.Path + "\n";
                break;
            case HeightMapAcquisitionFlag.Default:
                res += "Computing height map directly from OCAD file\n";
                break;
            case HeightMapAcquisitionFlag.Random:
                res += "Using random height map\n";
                break;
        }

        switch (DiffusionPointsOptions.Flag)
        {
            case DiffusionPointsAccusitionFlag.FromFile:
                res += "Diffusion points file: " + DiffusionPointsOptions.Path + "\n";
                break;
            case DiffusionPointsAccusitionFlag.Random:
                res += "Using random diffusion points\n";
                break;
            case DiffusionPointsAccusitionFlag.Manual:
                res += "Using manually set diffusion points: " + DiffusionPointsOptions.NumberOfPoints +
                       "\n";
                break;
        }

        return res;
    }
}