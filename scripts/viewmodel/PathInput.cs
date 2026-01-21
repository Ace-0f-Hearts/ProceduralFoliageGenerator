using Godot;
using System;

namespace ProceduralFoliageGenerator.ViewModel;
public partial class PathInput : LineEdit
{
    [Export]
    public string Extensions { get; set; }
    [Export]
    public string Description { get; set; }
    
    public string Path { get; set; } = "";
    
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

