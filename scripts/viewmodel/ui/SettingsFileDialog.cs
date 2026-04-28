using System;
using Godot;
using ProceduralFoliageGenerator.Model;

namespace ProceduralFoliageGenerator.ViewModel;

public partial class SettingsFileDialog : FileDialogController
{
    private const String DefaultGeneratorPath = "./Generator/App";

    public String CustomGeneratorPath {get; set; }
    
    
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

    public void OnCustomExecutableSet(String path)
    {
        CustomGeneratorPath = path;
        
    }
    
    public void OnConfirmPressed()
    {
        GenerationExecutor.Instance.GeneratorPath = CustomGeneratorPath;
        this.Hide();
    }

    public void OnCancelPressed()
    {
        this.Hide();
    }
}