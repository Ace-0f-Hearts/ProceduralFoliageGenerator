using Godot;
using System;
using ProceduralFoliageGenerator.Model;

public partial class FoliageModel : Node3D
{
    [Export]
    public Renderer Renderer { get; set; }
    public SpeciesLibrary SpeciesLibrary { get; set; }

    public override void _Ready()
    {
        GlobalModel.Instance.InUseSpeciesBuilder.BuilderReady += (obj, args) =>
        {
            SpeciesLibrary = GlobalModel.Instance.InUseSpeciesBuilder.Build();
            Renderer.FoliageRenderer.PopulateMultiMeshes(SpeciesLibrary);            
        };
        base._Ready();
    }
    
}
