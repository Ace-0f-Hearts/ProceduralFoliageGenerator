using Godot;
using System;
using ProceduralFoliageGenerator.Model;

public partial class Renderer : Node3D
{
    [Export]
    public FoliageRenderer FoliageRenderer { get; set; }
    [Export]
    public TerrainRenderer TerrainRenderer{ get; set;}

    public override void _Ready()
    {
        GlobalModel.Instance.FoliageController.BuilderReady += OnFoliageReady;

        base._Ready();
    }
    public void OnFoliageReady(object o, EventArgs args)
    {
        FoliageRenderer.Clear();
        
        TerrainRenderer.MapData = GlobalModel.Instance.FoliageController.Data.MapData;
        TerrainRenderer.SetBumpMap(GlobalModel.Instance.FoliageController.Data.HeightMap);
        TerrainRenderer.SetMapTexture(GlobalModel.Instance.FoliageController.Data.MapTexture);
        
        
        FoliageRenderer.MapData =  GlobalModel.Instance.FoliageController.Data.MapData;
        FoliageRenderer.PopulateMultiMeshes(GlobalModel.Instance.FoliageController.Build());
    }
}
