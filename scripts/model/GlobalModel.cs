

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using ProceduralFoliageGenerator.ViewModel;
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

    public FoliageController FoliageController { get; set; }

    public GenerationController GenerationController { get; set; }
    
    
    private GlobalModel()
    {
        FoliageController = new FoliageController();
        GenerationController = new();
        
        
    }
    
    public void ClearTemporaryGenerationData()
    {
        GenerationController.Clear();
    }


    public bool ExecuteGeneration()
    {
        var (execute,flags) = GenerationController.Execute();

        if (execute)
        {
            GenerationExecutor.Instance.GeneratorArguments = flags;
            GenerationExecutor.Instance.ExecuteGeneration();
        }
        return execute;
    }

    public void LoadGeneratedFoliage()
    {
        
        FoliageController.ParseConfig(GenerationController.LastGeneratedConfig);
        FoliageController.Populate();
    }


}