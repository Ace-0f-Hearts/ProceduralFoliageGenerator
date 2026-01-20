

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using FileAccess = System.IO.FileAccess;

namespace ProceduralFoliageGenerator.Model;
public class MainModel
{
    static private MainModel _instance = null;
    
    static public MainModel Instance 
    {
        get
        {
            if (_instance is null)
            {
                _instance = new MainModel();
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }


    public event EventHandler<ErrorEventArgs> ErrorOccured;     
    
    public DataStorage TemporaryData { get; set; }
    public DataStorage InUseData { get; set; }
    
    
    private FoliageDescriptorParser _foliageDescriptorParser;
    private PlantAttributeParser _plantAttrParser;
    
    private MainModel()
    {
        _foliageDescriptorParser = new FoliageDescriptorParser();
        _plantAttrParser = new PlantAttributeParser();
    }
    
    public void SetNewMapFile(string path)
    {
        TemporaryData.PathToMapFile = path;
    }
    
    public void SetNewPlantAttributeDescriptor(string path)
    {
        TemporaryData.PathToPlantFile = path;

        var content = File.ReadAllText(path);
        if (content.Length != 0)
        {
            TemporaryData.PlantAttrs = _plantAttrParser.Parse(content);
        }
        else
        {
            ErrorOccured?.Invoke(this,new ErrorEventArgs(new Exception("Plant species attribute descriptor file was empty.")));
        }
    }

    public void SetNewFoliageDescriptor(string path)
    {
        TemporaryData.PathToFoliageFile = path;
        var content = File.ReadAllText(path);
        if (content.Length != 0)
        {
            TemporaryData.PlantInstances = _foliageDescriptorParser.Parse(content);
        }
        else
        {
            ErrorOccured?.Invoke(this,new ErrorEventArgs(new Exception("Foliage descriptor file was empty, no instances loaded.")));
        }
        
    }


    public void StoreTemporyData()
    {
        InUseData = TemporaryData;
        TemporaryData = new DataStorage();
    }

    public void ClearTemporaryData()
    {
        TemporaryData.Clear();
    }
}