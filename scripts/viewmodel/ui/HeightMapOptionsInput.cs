using System;
using Godot;
using ProceduralFoliageGenerator.ViewModel;

public partial class HeightMapOptionsInput : TabContainer
{
    [Export] public PathInput HeightMapPathInput { get; set; }

    public HeightMapOptions Options { get; set; } = new();

    public event EventHandler OptionsReady;

    public override void _Ready()
    {
        HeightMapPathInput.TextSubmitted += text =>
        {
            Options.Path = text;
            CheckAndSignalWhenReady();
        };
        OnTabChanged(CurrentTab);
        base._Ready();
    }

    public void EnableInputs()
    {
        HeightMapPathInput.EnableButtons();
        SetTabDisabled(0, false);
        SetTabDisabled(1, false);
        // this.SetTabDisabled(2,false);
    }

    public void DisableInputs()
    {
        HeightMapPathInput.DisableButtons();
        SetTabDisabled(0, true);
        SetTabDisabled(1, true);
        // this.SetTabDisabled(2,true);
    }

    public void CheckAndSignalWhenReady()
    {
        if (Options.Ready())
        {
            GD.Print("Height Map ready!");
            OptionsReady?.Invoke(this, EventArgs.Empty);
        }
    }

    public void OnTabChanged(int i)
    {
        // if (i == 1)
        // {
        //     i = 0;
        //     this.SetCurrentTab(i);
        // }

        if (i == 0)
            Options.Flag = HeightMapAcquisitionFlag.Random;
        // else if (i == 1)
        //     Options.Flag = HeightMapAcquisitionFlag.Default;
        else if (i == 1)
            Options.Flag = HeightMapAcquisitionFlag.FromFile;

        CheckAndSignalWhenReady();
    }
}