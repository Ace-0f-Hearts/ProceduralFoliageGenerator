using Godot;
using System;
using ProceduralFoliageGenerator.Model;

public partial class Renderer : Node3D
{
    [Export]
    public FoliageRenderer FoliageRenderer { get; set; }
    [Export]
    public TerrainRenderer TerrainRenderer{ get; set;}
}
