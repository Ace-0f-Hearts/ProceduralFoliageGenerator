#nullable enable
using Godot;
using System;

using ProceduralFoliageGenerator.Model;

namespace ProceduralFoliageGenerator.ViewModel;

public partial class InputFileDialogController : FileDialogController
{

    [Export]
    public required PathInput MapFilePathInput { get; set; }
    [Export]
    public required PathInput AttributesPathInput { get; set; } 
    
    [Export]
    public required PathInput SymbolSetPathInput { get; set; }
    [Export]
    public required PathInput InstanceOutputPathInput { get; set; }
    
    [Export]
    public required PathInput HeightMapPathInput { get; set; }
    
    [Export]
    public required Label SizeOfMap { get; set; }
    [Export]
    public required Label MinElevation { get; set; }
    [Export]
    public required Label MaxElevation { get; set; }
    [Export]
    public required Label NumberOfPlants { get; set; }
    
    [Export]
    public required Container ButtonsContainer { get; set; }
    [Export]
    public required Container ProgressBarContainer { get; set; }

    public override void _Ready()
    {
        GlobalModel.Instance.TemporaryGenerationData.PlantAttributesSet += OnPlantAttributesSet;

        MapFilePathInput.FileDialogRequested += OnFileDialogReadRequested;
        AttributesPathInput.FileDialogRequested += OnFileDialogReadRequested;
        HeightMapPathInput.FileDialogRequested += OnFileDialogReadRequested;
        SymbolSetPathInput.FileDialogRequested += OnFileDialogReadRequested;
        
        InstanceOutputPathInput.FileDialogRequested += OnFileDialogWriteRequested;
        
        MapFilePathInput.TextSubmitted += OnMapSet;
        AttributesPathInput.TextSubmitted += OnAttributesFileSet;
        HeightMapPathInput.TextSubmitted += OnInstancesOutputSet;
        SymbolSetPathInput.TextSubmitted += OnSymbolSetSet;
        InstanceOutputPathInput.TextSubmitted += OnInstancesOutputSet;

        base._Ready();
    }


    public override void OnFileDialogOpenRequested(string extensions, string description, PathInput input)
    {
        MapFilePathInput.DisableButtons();
        AttributesPathInput.DisableButtons();
        InstanceOutputPathInput.DisableButtons();
        HeightMapPathInput.DisableButtons();
        SymbolSetPathInput.DisableButtons();
        base.OnFileDialogOpenRequested(extensions, description, input);
    }
    public override void OnFileDialogReadRequested(string extensions, string description, PathInput input)
    {
        base.OnFileDialogReadRequested(extensions, description, input);
        OnFileDialogOpenRequested(extensions, description, input);
    }
    
    public override void OnFileDialogWriteRequested(string extensions, string description, PathInput input)
    {
        base.OnFileDialogWriteRequested(extensions, description, input);
        OnFileDialogOpenRequested(extensions, description, input);
    }

    public override void OnFileDialogCloseRequested()
    {
        MapFilePathInput.EnableButtons();
        AttributesPathInput.EnableButtons();
        HeightMapPathInput.EnableButtons();
        SymbolSetPathInput.EnableButtons();
        InstanceOutputPathInput.EnableButtons();
        
        base.OnFileDialogCloseRequested();
    }

    public void OnGenerationStarted()
    {
        GlobalModel.Instance.StoreTemporaryGenerationData();
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
        GlobalModel.Instance.ClearTemporaryData();
        this.Hide();
    }

    public void OnMapSet(string input)
    {
        GlobalModel.Instance.SetNewMapFile(input);
    }

    public void OnAttributesFileSet(string input)
    {
        GlobalModel.Instance.SetPlantAttributesForGeneration(input);
    }

    public void OnInstancesOutputSet(string input)
    {
        GlobalModel.Instance.SetInstancesOutputPath(input);
    }

    public void OnSymbolSetSet(string input)
    {
        GlobalModel.Instance.SetSymbolSet(input);
    }

    public void OnHeightMapSet(string input)
    {
        GlobalModel.Instance.SetHeightMap(input);
    }

    public void OnPlantAttributesSet(object? o, EventArgs args)
    {
        this!.NumberOfPlants.Text = GlobalModel.Instance.TemporaryGenerationData.GetAmountOfSpecies.ToString();
    }
}




