using Godot;
using System;
using ProceduralFoliageGenerator.Model;

public partial class FoliageLoadingDialog : Control
{
    [Export] public Container InstanceAmountDisplay;
    [Export] public PackedScene InstanceAmountItem;

    public override void _Ready()
    {
        MainModel.Instance.TemporaryData.PlantInstancesLoaded += OnPlantInstancesLoaded;
        
        base._Ready();
    }

    private void OnPlantInstancesLoaded(object sender, EventArgs e)
    {
        var data = MainModel.Instance.TemporaryData.GetPlantInstanceAmountPerSpecies;

        foreach (var instance in data)
        {
            
        }
    }
}
