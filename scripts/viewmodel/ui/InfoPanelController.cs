using Godot;
using System;
using System.Collections.Generic;
using ProceduralFoliageGenerator.Model;
using ProceduralFoliageGenerator.ViewModel;

public partial class InfoPanelController : Control
{
    private List<Button> _buttons; 

    [Export]
    public required SpinBox NumberOfSymbols { get; set; }
    [Export]
    public required SpinBox NumberOfSpecies { get; set; }

    [Export] public SpeciesItem SpeciesItem { get; set; }
    
    [Export] GridContainer SpeciesGrid { get; set; }
    
    
    public override void _Ready()
    {
        _buttons = new();
        
        GlobalModel.Instance.GenerationController.InfoData.InfoChanged += OnInformationChanged;
        
        base._Ready();
    }
    
    public void OnInformationChanged(object? o, EventArgs args)
    {
        var data = GlobalModel.Instance.GenerationController.InfoData;
        
        this!.NumberOfSpecies.SetValue(data.NumberOfPlantAttributes);

        foreach (var attributes in data.PlantAttributes)
        {
            ListSpecies(attributes);
        }

        this!.NumberOfSymbols.SetValue(data.NumberOfSymbols);
    }

    public void ListSpecies(PlantAttributes attributes)
    {
        var button = new Button();
        button.Pressed += () =>
        {
            ShowSpecies(attributes);
        };
        button.SetText(attributes.Name);
        
        _buttons.Add(button);
        
        SpeciesGrid.AddChild(button);
    }

    public void ShowSpecies(PlantAttributes attributes)
    {
        SpeciesItem.UpdateInfo(attributes);
    }




    public void Clear()
    {
        foreach (var button in _buttons)
        {
            RemoveChild(button);
        }
        _buttons.Clear();
    }

}
