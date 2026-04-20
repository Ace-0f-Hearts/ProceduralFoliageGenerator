using Godot;
using System;
using Godot.Collections;
using ProceduralFoliageGenerator.ViewModel;
using Array = System.Array;

public partial class ControlUi : Control
{
    public bool IsGenerationPopupOpen { get; set; }
    public bool IsLoadingPopupOpen { get; set; }
    
    [Export]
    public Array<DialogPopupButton>  DialogPopupButtons { get; set; }
    [Export]
    public Array<FileDialogController> FileDialogControllers { get; set; }
    public override void _Ready()
    {

        int idx = 0;
        foreach (var button in (DialogPopupButtons))
        {
            int idxC = idx;
            button.Pressed += () =>
            {
                foreach (var d in FileDialogControllers)
                {
                    d.Hide();
                }
                FileDialogControllers[idxC].Show();
            };
            ++idx;
        }
        base._Ready();
    }
    
    
}
