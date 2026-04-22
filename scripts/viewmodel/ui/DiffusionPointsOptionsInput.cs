using Godot;
using System;
using ProceduralFoliageGenerator.ViewModel;

namespace ProceduralFoliageGenerator.ViewModel;
public partial class DiffusionPointsOptionsInput : TabContainer
{
    
    [Export]
    public PathInput DiffusionFilePathInput { get; set; }
    [Export]
    public SpinBox NumberOfPointsInput { get; set; }
    
    public DiffusionPointsOptions Options { get; set; } = new();
    
    public event EventHandler OptionsReady;

    public override void _Ready()
    {
        DiffusionFilePathInput.TextSubmitted += text =>
        {
            Options.Path = text;
            CheckAndSignalWhenReady();
        };

        NumberOfPointsInput.ValueChanged += value =>
        {
            Options.NumberOfPoints = (int)value;
            CheckAndSignalWhenReady();
        };
        OnTabChanged(this.CurrentTab);
    }

    public void EnableInputs()
    {
        DiffusionFilePathInput.EnableButtons();
        this.SetTabDisabled(0,false);
        this.SetTabDisabled(1,false);
        this.SetTabDisabled(2,false);    
    }

    public void DisableInputs()
    {
        DiffusionFilePathInput.DisableButtons();
        this.SetTabDisabled(0,true);
        this.SetTabDisabled(1,true);
        this.SetTabDisabled(2,true);
    }

    public void CheckAndSignalWhenReady()
    {
        if (Options.Ready())
        {
            GD.Print("DiffusionPoints Ready!");
            OptionsReady?.Invoke(this,EventArgs.Empty);
        }
    }

    public void OnTabChanged(int i)
    {
        CurrentTab = 0;
        i = 0;
        
        if (i == 0)
        {
            Options.Flag = DiffusionPointsAccusitionFlag.Random;
        } else if (i == 1)
        {
            Options.Flag = DiffusionPointsAccusitionFlag.Manual;
        }
        else if (i == 2)
        {
            Options.Flag = DiffusionPointsAccusitionFlag.FromFile;
        }
        CheckAndSignalWhenReady();
    }
    
}
