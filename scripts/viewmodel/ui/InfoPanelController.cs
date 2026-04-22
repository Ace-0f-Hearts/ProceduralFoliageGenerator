using Godot;
using System;
using ProceduralFoliageGenerator.Model;

public partial class InfoPanelController : Control
{
    [Export]
    public required Label SizeOfMap { get; set; }
    [Export]
    public required Label MinElevation { get; set; }
    [Export]
    public required Label MaxElevation { get; set; }
    [Export]
    public required Label NumberOfPlants { get; set; }
    
    public override void _Ready()
    {
        GlobalModel.Instance.GenerationController.InfoData.InfoChanged += OnPlantAttributesSet;
        
        base._Ready();
    }
    
    public void OnPlantAttributesSet(object? o, EventArgs args)
    {
        this!.NumberOfPlants.Text = GlobalModel.Instance.GenerationController.InfoData.GetAmountOfSpecies.ToString();
    }
}
