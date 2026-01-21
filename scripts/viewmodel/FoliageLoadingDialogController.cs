using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using ProceduralFoliageGenerator.Model;

namespace ProceduralFoliageGenerator.ViewModel;
public partial class FoliageLoadingDialogController : FileDialogController
{
    private const int InitialItemAmount = 5; 
        
    [Export] public Container InstanceAmountDisplay;
    [Export] public PackedScene InstanceAmountItem;

    [Export] public PathInput FoliageFilePathInput { get; set; }
    
    public List<InstanceAmountItem> Items;
    
    public override void _Ready()
    {
        MainModel.Instance.TemporaryData.PlantInstancesSet += OnPlantInstancesPreloaded;
        MainModel.Instance.InUseData.PlantInstancesSet += OnPlantInstancesLoaded;

        Items = new();
        
        for (var i = 0; i < InitialItemAmount; i++)
        {
            var item = InstanceAmountItem.Instantiate<InstanceAmountItem>();
            Items.Add(item);
            InstanceAmountDisplay.AddChild(item);
            item.Hide();
        }

        FoliageFilePathInput.FileDialogRequested += OnFileDialogOpenRequested;
        FoliageFilePathInput.TextSubmitted += OnFoliageFileInput;
        
        base._Ready();
    }

    public override void OnFileDialogOpenRequested(string extensions, string description, PathInput input)
    {
        FoliageFilePathInput.DisableButtons();
        base.OnFileDialogOpenRequested(extensions, description, input);
    }

    public override void OnFileDialogCloseRequested()
    {
        FoliageFilePathInput.EnableButtons();
        base.OnFileDialogCloseRequested();
    }

    private void OnFoliageFileInput(string path)
    {
        MainModel.Instance.SetNewFoliageDescriptor(path);
    }
    

    
    private void OnPlantInstancesPreloaded(object sender, EventArgs e)
    {
        var data = MainModel.Instance.TemporaryData.GetPlantInstanceAmountPerSpecies;

        //Add more items if necessary
        while (data.Count > Items.Count)
        {
            var item = InstanceAmountItem.Instantiate<InstanceAmountItem>();
            Items.Add(item);
            InstanceAmountDisplay.AddChild(item);
            item.Hide();
        }
        
        //Update descriptions on values on items
        foreach (var item in data.Select((x,i)  => new { Value = x, Index = i }))
        {
            var instance = item.Value;
            var idx = item.Index;
            Items[idx].SetDescription(instance.Item1);
            Items[idx].SetValue(instance.Item2);
            Items[idx].Show();
        }
        
    }

    public void OnPlantInstancesLoaded(object sender, EventArgs e)
    {
        // Remove all children from container
        foreach (var child in InstanceAmountDisplay.GetChildren())
        {
            (child as InstanceAmountItem)?.Hide();
        }
    }

    public void OnConfirmPressed()
    {
        FoliageFilePathInput.Clear();
        MainModel.Instance.StoreTemporyData();
        this.Hide();
    }

    public void OnCancelPressed()
    {
        FoliageFilePathInput.Clear();
        this.Hide();
    }
}
