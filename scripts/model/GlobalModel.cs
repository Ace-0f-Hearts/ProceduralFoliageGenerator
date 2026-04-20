

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using FileAccess = System.IO.FileAccess;

namespace ProceduralFoliageGenerator.Model;
public class GlobalModel
{
    static private GlobalModel _instance = null;
    
    static public GlobalModel Instance 
    {
        get
        {
            if (_instance is null)
            {
                _instance = new GlobalModel();
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }


    public event EventHandler<ErrorEventArgs> ErrorOccured;

    public SpeciesLibraryBuilder InUseSpeciesBuilder { get; set; }
    public SpeciesLibraryBuilder TemporaryInstanceData { get; set; }
    public GenerationCommandBuilder TemporaryGenerationData { get; set; }
    public GenerationCommandBuilder InUseGenerationData { get; set; }
    
    public List<PlantInstance>  PlantInstances { get; set; }
    
    private PlantInstanceParser _plantInstanceParser;
    private PlantAttributeParser _plantAttrParser;
    
    private GlobalModel()
    {
        TemporaryGenerationData = new GenerationCommandBuilder();
        InUseGenerationData = new GenerationCommandBuilder();
        
        TemporaryInstanceData = new SpeciesLibraryBuilder();
        InUseSpeciesBuilder = new SpeciesLibraryBuilder();
        
        _plantInstanceParser = new PlantInstanceParser();
        _plantAttrParser = new PlantAttributeParser();
    }
    
    public void SetNewMapFile(string path)
    {
        TemporaryGenerationData.PathToMapFile = path;
    }

    public void SetPlantAttributesForInstances(string path)
    {
        TemporaryInstanceData.PathToSpeciesAttributes = path;

        var content = File.ReadAllText(path);
        if (content.Length != 0)
        {
            TemporaryInstanceData.PlantAttributes = _plantAttrParser.Parse(content);
        }
        else
        {
            ErrorOccured?.Invoke(this,new ErrorEventArgs(new Exception("Plant species attribute descriptor file was empty.")));
        }
    }
    
    public void SetPlantAttributesForGeneration(string path)
    {
        TemporaryGenerationData.PathToSpeciesAttributes = path;

        var content = File.ReadAllText(path);
        if (content.Length != 0)
        {
            TemporaryGenerationData.PlantAttributes = _plantAttrParser.Parse(content);
        }
        else
        {
            ErrorOccured?.Invoke(this,new ErrorEventArgs(new Exception("Plant species attribute descriptor file was empty.")));
        }
    }

    public void SetSymbolSet(string path)
    {
        TemporaryGenerationData.PathToSymbolSet = path;
    }
    public void SetInstancesOutputPath(string path)
    {
        TemporaryGenerationData.PathToPlantInstances = path;
    }

    public void SetHeightMap(string path)
    {
        TemporaryGenerationData.PathToHeightMap = path;
    }
    
    public void SetNewInstances(string path)
    {
        TemporaryGenerationData.PathToPlantInstances = path;
        var content = File.ReadAllText(path);
        if (content.Length != 0)
        {
             var instances = _plantInstanceParser.Parse(content);
             TemporaryInstanceData.PlantInstances = instances;
        }
        else
        {
            ErrorOccured?.Invoke(this,new ErrorEventArgs(new Exception("Foliage descriptor file was empty, no instances loaded.")));
        }
        
    }

    public void StoreTemporaryInstanceData()
    {
        InUseSpeciesBuilder = TemporaryInstanceData;
        TemporaryInstanceData =  new();
    }

    public void StoreTemporaryGenerationData()
    {
        InUseGenerationData = TemporaryGenerationData;
        TemporaryGenerationData = new GenerationCommandBuilder();
    }

    public void ClearTemporaryData()
    {
        TemporaryGenerationData.Clear();
    }
}