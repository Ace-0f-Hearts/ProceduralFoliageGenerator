using Godot;

namespace ProceduralFoliageGenerator.ViewModel;

public record DiffusionPoint
{
    public Vector3 Position { get; set; }
    public float Radius { get; set; }
}