using Godot;
using ProceduralFoliageGenerator.Model;

namespace ProceduralFoliageGenerator.ViewModel;

public partial class SpeciesItem : Control
{
    [Export] public RangeItem GrowthRange { get; set; }

    [Export] public GaussianItem Elevation { get; set; }

    [Export] public GaussianItem Slope { get; set; }

    public void UpdateInfo(PlantAttributes plantAttrib)
    {
        GrowthRange.UpdateInfo(plantAttrib.GrowthRadius);
        Elevation.UpdateInfo(plantAttrib.Elevation);
        Slope.UpdateInfo(plantAttrib.Slope);
    }
}