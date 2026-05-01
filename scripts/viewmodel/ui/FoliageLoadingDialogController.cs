using System.Collections.Generic;
using Godot;
using ProceduralFoliageGenerator.Model;

namespace ProceduralFoliageGenerator.ViewModel;

public partial class FoliageLoadingDialogController : FileDialogController
{

    [Export] public PathInput FoliageConfigInput { get; set; }

    public override void _Ready()
    {
        FoliageConfigInput.FileDialogRequested += OnFileDialogReadRequested;
        FoliageConfigInput.TextSubmitted += OnConfigInput;

        base._Ready();
    }

    public override void OnFileDialogOpenRequested(string extensions, string description, PathInput input)
    {
        FoliageConfigInput.DisableButtons();
        base.OnFileDialogOpenRequested(extensions, description, input);
    }

    public override void OnFileDialogReadRequested(string extensions, string description, PathInput input)
    {
        base.OnFileDialogReadRequested(extensions, description, input);
        OnFileDialogOpenRequested(extensions, description, input);
    }

    public override void OnFileDialogCloseRequested()
    {
        FoliageConfigInput.EnableButtons();
        base.OnFileDialogCloseRequested();
    }

    private void OnConfigInput(string path)
    {
        GlobalModel.Instance.FoliageController.ParseConfig(path);
    }


    public void OnConfirmPressed()
    {
        FoliageConfigInput.Clear();
        GlobalModel.Instance.FoliageController.Populate();

        Hide();
    }

    public void OnCancelPressed()
    {
        FoliageConfigInput.Clear();
        Hide();
    }
}