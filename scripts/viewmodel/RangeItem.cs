using Godot;
using ProceduralFoliageGenerator.Model;

namespace ProceduralFoliageGenerator.ViewModel;

public partial class RangeItem : Control
{
    [Export] public SpinBox Min { get; set; }

    [Export] public SpinBox Max { get; set; }

    public void UpdateInfo(RangeValue range)
    {
        Min.SetValue(range.Min);
        Max.SetValue(range.Max);
    }
}