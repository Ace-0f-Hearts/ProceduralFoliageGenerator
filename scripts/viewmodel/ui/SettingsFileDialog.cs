using Godot;

namespace ProceduralFoliageGenerator.ViewModel;

public partial class SettingsFileDialog : FileDialogController
{
    private const string DefaultGeneratorPath = "./Generator/App";

    public string CustomGeneratorPath { get; set; }


    [Export] public PathInput CustomGeneratorPathInput { get; set; }

    public override void _Ready()
    {
        CustomGeneratorPathInput.FileDialogRequested += OnFileDialogReadRequested;
        CustomGeneratorPathInput.TextSubmitted += OnCustomExecutableSet;


        CustomGeneratorPathInput.Path = DefaultGeneratorPath;
    }

    public override void OnFileDialogOpenRequested(string extensions, string description, PathInput input)
    {
        DisableInputs();
        base.OnFileDialogOpenRequested(extensions, description, input);
    }

    public override void OnFileDialogReadRequested(string extensions, string description, PathInput input)
    {
        base.OnFileDialogReadRequested(extensions, description, input);
        OnFileDialogOpenRequested(extensions, description, input);
    }

    public override void OnFileDialogCloseRequested()
    {
        EnableInputs();
        base.OnFileDialogCloseRequested();
    }

    public void DisableInputs()
    {
        CustomGeneratorPathInput.DisableButtons();
    }

    public void EnableInputs()
    {
        CustomGeneratorPathInput.EnableButtons();
    }

    public void OnCustomExecutableSet(string path)
    {
        CustomGeneratorPath = path;
    }

    public void OnConfirmPressed()
    {
        GenerationExecutor.Instance.GeneratorPath = CustomGeneratorPath;
        Hide();
    }

    public void OnCancelPressed()
    {
        Hide();
    }
}