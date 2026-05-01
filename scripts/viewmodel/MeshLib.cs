using System.Linq;
using Godot;
using Godot.Collections;
using ProceduralFoliageGenerator.Model;

public partial class MeshLib : Node3D
{
    [Export] public Array<PlantObject> Plants { get; set; }

    public override void _Ready()
    {
        GlobalModel.Instance.FoliageController.PlantObjects = Plants.ToList();
        base._Ready();
    }
}