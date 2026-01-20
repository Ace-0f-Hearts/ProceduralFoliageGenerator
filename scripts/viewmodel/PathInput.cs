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
    public FileDialogController Controller { get; private set; }

    [Export]
    public Button FileDialogRequestButton { get; private set; }
    
    public override void _Ready()
    {
        FileDialogRequestButton!.Pressed += () => { Controller!.OnFileDialogOpenRequested(Extensions, Description, this); };
    }
}

