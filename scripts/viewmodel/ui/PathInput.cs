using Godot;
using System;

namespace ProceduralFoliageGenerator.ViewModel;
public partial class PathInput : LineEdit
{
    private String _path;
    
    [Export]
    public string Extensions { get; set; }
    [Export]
    public string Description { get; set; }

    public String Path
    {
        get => _path;
        set
        {
            _path = value;
            this.Text = _path;
            this.EmitSignal(LineEdit.SignalName.TextSubmitted, this);
        }
    }
    
    [Export]
    public Button FileDialogRequestButton { get; private set; }

    public event Action<string,string,PathInput> FileDialogRequested ;
    
    public override void _Ready()
    {
        FileDialogRequestButton!.Pressed += () => { FileDialogRequested?.Invoke(Extensions, Description, this); };
    }

    public void EnableButtons()
    {
        this.FileDialogRequestButton.Disabled = false;
    }

    public void DisableButtons()
    {
        this.FileDialogRequestButton.Disabled = true;
    }
    
}

