#nullable enable
using Godot;
using System;

using ProceduralFoliageGenerator.Model;

namespace ProceduralFoliageGenerator.ViewModel;

public partial class InputFileDialogController : FileDialogController
{

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
    
    [Export]
    public Container ButtonsContainer { get; set; }
    [Export]
    public Container ProgressBarContainer { get; set; }

    public override void _Ready()
    {
        MainModel.Instance.TemporaryData.PlantAttributesSet += OnPlantAttributesSet;

        MapFilePathInput.FileDialogRequested += OnFileDialogOpenRequested;
        PlantFilePathInput.FileDialogRequested += OnFileDialogOpenRequested;
        MapFilePathInput.TextSubmitted += OnMapFileInput;
        PlantFilePathInput.TextSubmitted += OnPlantFileInput;
        base._Ready();
    }

    public override void OnFileDialogOpenRequested(string extensions, string description, PathInput input)
    {
        MapFilePathInput.DisableButtons();
        PlantFilePathInput.DisableButtons();
        base.OnFileDialogOpenRequested(extensions, description, input);
    }

    public override void OnFileDialogCloseRequested()
    {
        MapFilePathInput.EnableButtons();
        MapFilePathInput.DisableButtons();
        
        base.OnFileDialogCloseRequested();
    }

    public void OnGenerationStarted()
    {
        MainModel.Instance.StoreTemporyData();
        GenerationExecutor.Instance.ExecuteGeneration();
        ButtonsContainer.Hide();
        ProgressBarContainer.Show();
    }

    public void OnGenerationProgressed(float progress)
    {
        //TODO: Update progressbar   
    }

    public void OnGenerationCompleted()
    {
        ButtonsContainer.Show();
        ProgressBarContainer.Hide();
        this.Hide();
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




