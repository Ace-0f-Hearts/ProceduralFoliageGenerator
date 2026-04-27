using System;
using System.Collections.Generic;
using Godot;

namespace ProceduralFoliageGenerator.Model;

public partial class FoliageRenderer : Node3D
{
    public MapData MapData { get; set; }
    public List<MultiMeshInstance3D> MultiMeshes { get; init; } = new();


    public override void _Ready()
    {
        base._Ready();
    }

    public void PopulateMultiMeshes(Foliage lib)
    {
        Clear();
        foreach (var (obj,instances) in lib.GetInstancesPerObjects())
        {
            RequestNewMultiMesh(obj,instances);
        }
    }
    
    private void RequestNewMultiMesh(PlantObject plant, List<PlantInstance> instances)
    {
        // var trunkMultiMesh = new MultiMesh();
        // trunkMultiMesh.TransformFormat = MultiMesh.TransformFormatEnum.Transform3D;
        // trunkMultiMesh.InstanceCount = instances.Count;
        // trunkMultiMesh.VisibleInstanceCount = -1; // Draw all instances
        //
        // var foliageMultiMesh = new MultiMesh();
        // foliageMultiMesh.TransformFormat = MultiMesh.TransformFormatEnum.Transform3D;
        // foliageMultiMesh.InstanceCount = instances.Count;
        // foliageMultiMesh.VisibleInstanceCount = -1;
        //
        // trunkMultiMesh.SetMesh(plant.TrunkMesh);
        // foliageMultiMesh.SetMesh(plant.FoliageMesh);
        // int idx = 0;
        // foreach (var instance in instances)
        // {
        //     trunkMultiMesh.SetInstanceTransform(idx,new Transform3D(Basis.Identity.Scaled(new Vector3(instance.Scale / MapData.Scaling / 10.0f,instance.Scale / MapData.Scaling / 10.0f,instance.Scale / MapData.Scaling / 10.0f)) ,instance.WorldPosition / MapData.Scaling));
        //     foliageMultiMesh.SetInstanceTransform(idx,new Transform3D(Basis.Identity.Scaled(new Vector3(instance.Scale / MapData.Scaling / 10.0f,instance.Scale / MapData.Scaling / 10.0f,instance.Scale / MapData.Scaling / 10.0f)) ,instance.WorldPosition / MapData.Scaling));
        //     ++idx;
        // }
        //
        // var trunkMultiMeshInstance = new MultiMeshInstance3D();
        // var foliageMultiMeshInstance = new MultiMeshInstance3D();
        // trunkMultiMeshInstance.SetMultimesh(trunkMultiMesh);
        // foliageMultiMeshInstance.SetMultimesh(trunkMultiMesh);
        // this.AddChild(trunkMultiMeshInstance);
        // this.AddChild(foliageMultiMeshInstance);
        // MultiMeshes.Add(trunkMultiMeshInstance);
        // MultiMeshes.Add(foliageMultiMeshInstance);

        foreach (var instance in instances)
        {
            
            MeshInstance3D trunk =  new ();
            trunk.SetMesh(plant.TrunkMesh);
            MeshInstance3D foliage = new();
            foliage.SetMesh(plant.FoliageMesh);

            Sprite3D sprite = new();
            sprite.SetTexture(plant.PlantTexture);
            sprite.Billboard = BaseMaterial3D.BillboardModeEnum.Enabled;
            sprite.AlphaCut = SpriteBase3D.AlphaCutMode.Discard;
            
            
            
            var random = new RandomNumberGenerator();

            var angle = random.RandfRange(0,2 * Single.Pi);
            var origin = instance.WorldPosition / MapData.Scaling;
            var basis = Basis.Identity;
            basis = basis.Scaled(new Vector3(instance.Scale / MapData.Scaling / 10.0f,
                instance.Scale / MapData.Scaling / 10.0f, instance.Scale / MapData.Scaling / 10.0f));

            basis = basis.Rotated(Vector3.Up,angle);
            
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
            
            
            this.AddChild(trunk);
            this.AddChild(foliage);
            this.AddChild(sprite);
            
        }

    }

    public void Clear()
    {
        foreach (var multiMesh in MultiMeshes)
        {
            this.RemoveChild(multiMesh);
            multiMesh.Dispose();
        }
        MultiMeshes.Clear();
    }

 
    
}