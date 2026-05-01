using Godot;
using ProceduralFoliageGenerator.Model;

namespace ProceduralFoliageGenerator.ViewModel;

public partial class InputPanelController : FileDialogController
{
    [Export] public required PathInput MapFilePathInput { get; set; }

    [Export] public required HeightMapOptionsInput HeightMapOptions { get; set; }

    [Export] public required DiffusionPointsOptionsInput DiffusionPointsOptions { get; set; }

    [Export] public required PathInput AttributesPathInput { get; set; }

    [Export] public required PathInput SymbolSetPathInput { get; set; }

    [Export] public required PathInput InstanceOutputPathInput { get; set; }


    public override void _Ready()
    {
        MapFilePathInput.FileDialogRequested += OnFileDialogReadRequested;
        AttributesPathInput.FileDialogRequested += OnFileDialogReadRequested;
        SymbolSetPathInput.FileDialogRequested += OnFileDialogReadRequested;
        InstanceOutputPathInput.FileDialogRequested += OnFileDialogWriteRequested;

        HeightMapOptions.HeightMapPathInput.FileDialogRequested += OnFileDialogReadRequested;
        // DiffusionPointsOptions.DiffusionFilePathInput.FileDialogRequested += OnFileDialogReadRequested;

        MapFilePathInput.TextSubmitted += OnMapSet;
        AttributesPathInput.TextSubmitted += OnAttributesFileSet;
        SymbolSetPathInput.TextSubmitted += OnSymbolSetSet;
        InstanceOutputPathInput.TextSubmitted += OnInstancesOutputSet;

        HeightMapOptions.OptionsReady += (o, args) => OnHeightMapSet(HeightMapOptions.Options);
        DiffusionPointsOptions.OptionsReady += (o, args) => OnDiffusionPointsSet(DiffusionPointsOptions.Options);

        HeightMapOptions.CheckAndSignalWhenReady();
        DiffusionPointsOptions.CheckAndSignalWhenReady();

        InstanceOutputPathInput.Path = "instances.json";

        base._Ready();
    }

    public override void OnFileDialogOpenRequested(string extensions, string description, PathInput input)
    {
        MapFilePathInput.DisableButtons();
        AttributesPathInput.DisableButtons();
        InstanceOutputPathInput.DisableButtons();
        SymbolSetPathInput.DisableButtons();

        HeightMapOptions.DisableInputs();
        DiffusionPointsOptions.DisableInputs();

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
        HeightMapOptions.EnableInputs();
        SymbolSetPathInput.EnableButtons();
        InstanceOutputPathInput.EnableButtons();

        HeightMapOptions.EnableInputs();
        DiffusionPointsOptions.EnableInputs();

        base.OnFileDialogCloseRequested();
    }

    public void OnMapSet(string input)
    {
        GlobalModel.Instance.GenerationController.CommandData.PathToMapFile = input;
    }

    public void OnAttributesFileSet(string input)
    {
        GlobalModel.Instance.GenerationController.CommandData.PathToSpeciesAttributes = input;
    }

    public void OnInstancesOutputSet(string input)
    {
        GlobalModel.Instance.GenerationController.CommandData.PathToPlantInstances = input;
    }

    public void OnSymbolSetSet(string input)
    {
        GlobalModel.Instance.GenerationController.CommandData.PathToSymbolSet = input;
    }

    public void OnHeightMapSet(HeightMapOptions options)
    {
        GlobalModel.Instance.GenerationController.CommandData.HeightMapOptions = options;
    }

    public void OnDiffusionPointsSet(DiffusionPointsOptions options)
    {
        GlobalModel.Instance.GenerationController.CommandData.DiffusionPointsOptions = options;
    }
}