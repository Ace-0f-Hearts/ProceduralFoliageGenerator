using System;
using System.IO;
using ProceduralFoliageGenerator.ViewModel;

namespace ProceduralFoliageGenerator.Model;

public class GlobalModel
{
    private static GlobalModel _instance;


    private GlobalModel()
    {
        FoliageController = new FoliageController();
        GenerationController = new GenerationController();
    }

    public static GlobalModel Instance
    {
        get
        {
            if (_instance is null) _instance = new GlobalModel();
            return _instance;
        }
        private set => _instance = value;
    }

    public FoliageController FoliageController { get; set; }

    public GenerationController GenerationController { get; set; }


    public event EventHandler<ErrorEventArgs> ErrorOccured;

    public void ClearTemporaryGenerationData()
    {
        GenerationController.Clear();
    }


    public bool ExecuteGeneration()
    {
        var (execute, flags) = GenerationController.Execute();

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