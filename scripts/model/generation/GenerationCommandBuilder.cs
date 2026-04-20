using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace ProceduralFoliageGenerator.Model;

/// <summary>
/// Structure for storing all foliage rendering and generation related data.
/// </summary>
public record GenerationCommandBuilder
{
    public event EventHandler PlantAttributesSet;
    public event EventHandler<string> BuildRequestedWhileNotReady;
    
    private List<PlantAttributes> _plantAttributes;

    public String PathToSymbolSet { get; set; }
    public String PathToMapFile { get; set; }
    public String PathToSpeciesAttributes { get; set; }

    public String PathToHeightMap { get; set; }
    public String PathToPlantInstances { get; set; }
    
    public bool UseRandomDiffusionPoints { get; set; } = true;
    public int NumberOfRandomDiffusionPoints { get; set; } = 3;
    public bool UseHeightMap { get; set; } = false;
    
    public GenerationCommandBuilder()
    {
        _plantAttributes = new();
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

    private bool IsReady()
    {
        var predicates = 
        new List<(Func<bool>,string)>{
            (() => { return Path.Exists(PathToSymbolSet); },"Issue with file containing the symbol set: File does not exist\n"),
            (() => { return Path.HasExtension(PathToSymbolSet) && Path.GetExtension(PathToSymbolSet) == ".json"; },"Issue with file containing the symbol set: Extension is missing or not supported. Supported extensions: '.json'\n"),
            (() => { return Path.Exists(PathToMapFile);},"Issue with OCAD map file: File does not exist\n"),
            (() => { return Path.HasExtension(PathToMapFile) && (Path.GetExtension(PathToMapFile) == ".ocad" || Path.GetExtension(PathToMapFile) == ".ocd");},"Issue with OCAD map file: Extension is missing or not supported. Supported extensions: '.ocad', '.ocd'\n"),
            (() => { return Path.Exists(PathToSpeciesAttributes);},"Issue with file containing species attributes: File does not exist\n"),
            (() => { return Path.HasExtension(PathToSpeciesAttributes) && Path.GetExtension(PathToSpeciesAttributes) == ".json"; },"Issue with file containing species attributes: Extension is missing or not supported. Supported extensions: '.json'\n"),
            (() => { return (PathToHeightMap.Length == 0 || (Path.Exists(PathToHeightMap)));},"Issue with height map: File does not exist\n"),
            (() => { return Path.Exists(PathToPlantInstances);},"Issue with output path to instances: Path does not exist\n")
            
        };
        bool isReady = true;

        List<String> errors = new();
        
        foreach (var (predicate,errorMessage) in predicates)
        {
            if (!predicate())
            {
                isReady = false;
                errors.Add(errorMessage);
            }
        }
        
        return isReady;
    }
    
    public (bool, string) Build()
    {
        var buildInstructions = new List<(string, Func<string>)>
        {
            ("--ocad",() => PathToMapFile),
            ("--species", () => PathToSpeciesAttributes),
            ("--heightmap", () => PathToHeightMap),
            ("--symbol", () => PathToSymbolSet),
            ("--out", () => PathToPlantInstances),
            ("--random_diff", () => NumberOfRandomDiffusionPoints.ToString())
        };
        
        bool isReady = IsReady();
        String command = String.Empty;
        if (isReady) 
        {
            foreach (var (option,instruction) in buildInstructions)
            {
                command += option + " " + instruction();
            }
        }
        return (isReady, command);

    }

    public int GetAmountOfSpecies => PlantAttributes.Count;

    public void Clear()
    {
        _plantAttributes.Clear();
        PathToMapFile = String.Empty;
        PathToSpeciesAttributes = String.Empty;
        PathToPlantInstances = String.Empty;
        PathToHeightMap = String.Empty;
        PathToSymbolSet = String.Empty;
        
        UseHeightMap = false;
        UseRandomDiffusionPoints = true;
    }
}