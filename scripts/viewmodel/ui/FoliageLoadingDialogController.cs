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

    [Export] public PathInput InstancesPathInput { get; set; }
    [Export] public PathInput AttributesPathInput { get; set; }
    
    public List<InstanceAmountItem> Items;
    
    public override void _Ready()
    {
        GlobalModel.Instance.TemporaryInstanceData.PlantInstancesSet += OnPlantInstancesPreloaded;
        GlobalModel.Instance.InUseSpeciesBuilder.PlantInstancesSet += OnPlantInstancesLoaded;

        Items = new();
        
        for (var i = 0; i < InitialItemAmount; i++)
        {
            var item = InstanceAmountItem.Instantiate<InstanceAmountItem>();
            Items.Add(item);
            InstanceAmountDisplay.AddChild(item);
            item.Hide();
        }

        InstancesPathInput.FileDialogRequested += OnFileDialogReadRequested;
        InstancesPathInput.TextSubmitted += OnInstancesInput;
        

        AttributesPathInput.FileDialogRequested += OnFileDialogReadRequested;
        AttributesPathInput.TextSubmitted += OnAttributesInput;

        GlobalModel.Instance.InUseGenerationData.PlantAttributesSet += SetLoadedAttributePath;
        // FileDialog.CloseRequested += OnFileDialogCloseRequested;
        
        base._Ready();
    }

    public void SetLoadedAttributePath(object sender, EventArgs e)
    {
        AttributesPathInput.Path = GlobalModel.Instance.InUseGenerationData.PathToSpeciesAttributes;
    }

    
    
    public override void OnFileDialogOpenRequested(string extensions, string description, PathInput input)
    {
        InstancesPathInput.DisableButtons();
        AttributesPathInput.DisableButtons();
        // FileDialog.CloseRequested += OnFileDialogCloseRequested;
        base.OnFileDialogOpenRequested(extensions, description, input);
    }

    public override void OnFileDialogReadRequested(string extensions, string description, PathInput input)
    {
        base.OnFileDialogReadRequested(extensions, description, input);
        OnFileDialogOpenRequested(extensions, description, input);
    }

    public override void OnFileDialogCloseRequested()
    {
        InstancesPathInput.EnableButtons();
        AttributesPathInput.EnableButtons();
        base.OnFileDialogCloseRequested();
    }

    private void OnInstancesInput(string path)
    {
        GlobalModel.Instance.SetNewInstances(path);
    }

    private void OnAttributesInput(string path)
    {
        GlobalModel.Instance.SetPlantAttributesForInstances(path);
    }
    

    
    private void OnPlantInstancesPreloaded(object sender, EventArgs e)
    {
        var data = GlobalModel.Instance.TemporaryInstanceData.GetNumberOfInstancesPerSpecies;

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
        InstancesPathInput.Clear();
        GlobalModel.Instance.StoreTemporaryInstanceData();
        this.Hide();
    }

    public void OnCancelPressed()
    {
        InstancesPathInput.Clear();
        this.Hide();
    }
}
