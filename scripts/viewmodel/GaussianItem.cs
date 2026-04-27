using Godot;
using System;
using ProceduralFoliageGenerator.Model;

public partial class GaussianItem : PanelContainer
{
    [Export]
    public SpinBox Peak {get; set;}
    [Export]
    public SpinBox Avg {get; set;}
    [Export]
    public SpinBox StdDev {get; set;}

    public void UpdateInfo(Gaussian gaussian)
    {
        Peak.SetValue(gaussian.Peak);
        Avg.SetValue(gaussian.Avg);
        StdDev.SetValue(gaussian.Deviation);
    }
}
