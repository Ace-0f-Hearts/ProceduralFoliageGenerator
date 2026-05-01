using System.Collections.Generic;
using Godot;

namespace ProceduralFoliageGenerator.Model;

public partial class FoliageRenderer : Node3D
{
    public MapData MapData { get; set; }
    public List<MeshInstance3D> Instances { get; init; } = new();
    public List<Sprite3D> Sprites { get; init; } = new();


    public override void _Ready()
    {
        base._Ready();
    }

    public void PopulateMultiMeshes(Foliage lib)
    {
        Clear();
        foreach (var (obj, instances) in lib.GetInstancesPerObjects()) RequestNewMultiMesh(obj, instances);
    }

    private void RequestNewMultiMesh(PlantObject plant, List<PlantInstance> instances)
    {
        foreach (var instance in instances)
        {
            MeshInstance3D trunk = new();
            trunk.SetMesh(plant.TrunkMesh);
            MeshInstance3D foliage = new();
            foliage.SetMesh(plant.FoliageMesh);

            Sprite3D sprite = new();
            sprite.SetTexture(plant.PlantTexture);
            sprite.Billboard = BaseMaterial3D.BillboardModeEnum.Enabled;
            sprite.AlphaCut = SpriteBase3D.AlphaCutMode.Discard;
            
            var random = new RandomNumberGenerator();

            var angle = random.RandfRange(0, 2 * float.Pi);
            var origin = instance.WorldPosition / MapData.Scaling;
            var basis = Basis.Identity;
            basis = basis.Scaled(new Vector3(instance.Scale / MapData.Scaling / 10.0f,
                instance.Scale / MapData.Scaling / 10.0f, instance.Scale / MapData.Scaling / 10.0f));

            basis = basis.Rotated(Vector3.Up, angle);

            var basis2 = Basis.Identity;
            basis2 = basis2.Scaled(new Vector3(instance.Scale / MapData.Scaling / 1.0f,
                instance.Scale / MapData.Scaling / 1.0f, instance.Scale / MapData.Scaling / 1.0f));

            var transform = new Transform3D(basis, origin);
            var transform2 = new Transform3D(basis2, origin);
            trunk.SetTransform(transform);
            foliage.SetTransform(transform);
            sprite.SetTransform(transform2);

            trunk.VisibilityRangeFadeMode = GeometryInstance3D.VisibilityRangeFadeModeEnum.Disabled;
            foliage.VisibilityRangeFadeMode = GeometryInstance3D.VisibilityRangeFadeModeEnum.Disabled;

            trunk.VisibilityRangeEnd = 3;
            trunk.VisibilityRangeEndMargin = 1;
            foliage.VisibilityRangeEnd = 3;
            foliage.VisibilityRangeEndMargin = 1;

            sprite.VisibilityRangeFadeMode = GeometryInstance3D.VisibilityRangeFadeModeEnum.Disabled;
            sprite.VisibilityRangeBegin = 3;
            sprite.VisibilityRangeBeginMargin = 1;

            sprite.VisibilityRangeEnd = 35;
            sprite.VisibilityRangeEndMargin = 5;

            Instances.Add(trunk);
            Instances.Add(foliage);
            Sprites.Add(sprite);

            AddChild(trunk);
            AddChild(foliage);
            AddChild(sprite);
        }
    }

    public void Clear()
    {
        foreach (var instance in Instances) instance.QueueFree();

        foreach (var sprite in Sprites) sprite.QueueFree();

        Instances.Clear();
        Sprites.Clear();
    }
}