using System;
using Godot;

namespace ProceduralFoliageGenerator.ViewModel;

public partial class PathInput : LineEdit
{
    private string _path;

    [Export] public string Extensions { get; set; }

    [Export] public string Description { get; set; }

    public string Path
    {
        get => _path;
        set
        {
            _path = value;
            Text = _path;
            EmitSignal(LineEdit.SignalName.TextSubmitted, _path);
        }
    }

    [Export] public Button FileDialogRequestButton { get; private set; }

    public event Action<string, string, PathInput> FileDialogRequested;

    public override void _Ready()
    {
        FileDialogRequestButton!.Pressed += () => { FileDialogRequested?.Invoke(Extensions, Description, this); };
    }

    public void EnableButtons()
    {
        FileDialogRequestButton.Disabled = false;
    }

    public void DisableButtons()
    {
        FileDialogRequestButton.Disabled = true;
    }
}