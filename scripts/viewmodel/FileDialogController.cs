#nullable enable
using Godot;
using System;

using ProceduralFoliageGenerator.Model;

namespace ProceduralFoliageGenerator.ViewModel;

public partial class FileDialogController : Control
{
    [Export]
    public FileDialog FileDialog { get; set; }
    
    [Export]
    public PathInput MapFilePathInput { get; set; }
    
    [Export]
    public PathInput PlantFilePathInput { get; set; } 
    
    [Export]
    public Label SizeOfMap { get; set; }
    [Export]
    public Label MinElevation { get; set; }
    [Export]
    public Label MaxElevation { get; set; }
    [Export]
    public Label NumberOfPlants { get; set; }
    

    public override void _Ready()
    {
        MainModel.Instance.TemporaryData.PlantAttributesSet += OnPlantAttributesSet;
        
        FileDialog.CloseRequested += OnFileDialogCloseRequested;
        MapFilePathInput.TextSubmitted += OnMapFileInput;
        PlantFilePathInput.TextSubmitted += OnPlantFileInput;
    }
    
    /// <summary>
    /// Signal handler for handling requests of opening the file explorer node.
    /// </summary>
    /// <param name="extensions"></param>
    /// <param name="description"></param>
    /// <param name="input"></param>
    public void OnFileDialogOpenRequested(string extensions,string description,PathInput input)
    {
        FileDialog.AddFilter(extensions,description);
        FileDialog.FileSelected += (path) => OnFileSelected(path,input);
        FileDialog.Show();
    }

    
    /// <summary>
    /// Signal handler for handling the requests of closing the file explorer node.
    /// </summary>
    public void OnFileDialogCloseRequested()
    {
        FileDialog.ClearFilters();
        FileDialog.CurrentFile = "";
        FileDialog.Hide();
    }

    public void OnFileSelected(string path,PathInput input)
    {
        FileDialog.ClearFilters();
        input.Text = path;
        input.EmitSignal(LineEdit.SignalName.TextSubmitted, path);
        FileDialog.Hide();
    }

    public void OnGenerationStarted()
    {
        MainModel.Instance.StoreTemporyData();
        GenerationExecutor.Instance.ExecuteGeneration();
    }

    public void OnGenerationCanceled()
    {
        MainModel.Instance.ClearTemporaryData();
        this.Hide();
    }

    public void OnMapFileInput(string input)
    {
        MainModel.Instance.SetNewMapFile(input);
    }

    public void OnPlantFileInput(string input)
    {
        MainModel.Instance.SetNewPlantAttributeDescriptor(input);
    }

    public void OnPlantAttributesSet(object? o, EventArgs args)
    {
        this!.NumberOfPlants.Text = MainModel.Instance.TemporaryData.GetAmountOfSpecies.ToString();
    }
}



