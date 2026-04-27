using Godot;
using System;
using ProceduralFoliageGenerator.Model;

public partial class FoliageModel : Node3D
{
    [Export]
    public Renderer Renderer { get; set; }
    public Foliage Foliage { get; set; }

    public override void _Ready()
    {
        GlobalModel.Instance.FoliageController.BuilderReady += (obj, args) =>
        {
            Foliage = GlobalModel.Instance.FoliageController.Build();
        };
        base._Ready();
    }
    
}
