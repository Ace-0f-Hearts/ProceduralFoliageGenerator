using Godot;
using Godot.Collections;
using ProceduralFoliageGenerator.ViewModel;

public partial class ControlUi : Control
{
    public bool IsGenerationPopupOpen { get; set; }
    public bool IsLoadingPopupOpen { get; set; }

    public bool IsSettingsPopupOpen { get; set; }

    [Export] public Array<DialogPopupButton> DialogPopupButtons { get; set; }

    [Export] public Array<Control> FileDialogControllers { get; set; }

    public override void _Ready()
    {
        var idx = 0;
        foreach (var button in DialogPopupButtons)
        {
            var idxC = idx;
            button.Pressed += () =>
            {
                foreach (var d in FileDialogControllers) d.Hide();
                FileDialogControllers[idxC].Show();
            };
            ++idx;
        }

        base._Ready();
    }
}