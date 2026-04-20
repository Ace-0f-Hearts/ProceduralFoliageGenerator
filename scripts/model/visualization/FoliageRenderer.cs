using System.Collections.Generic;
using Godot;

namespace ProceduralFoliageGenerator.Model;

public partial class FoliageRenderer : Node3D
{
    public List<MultiMeshInstance3D> MultiMeshes { get; init; }

    public void PopulateMultiMeshes(SpeciesLibrary lib)
    {
        foreach (var (obj,instances) in lib.GetInstancesPerObjects())
        {
            RequestNewMultiMesh(obj,instances);
        }
    }
    
    private void RequestNewMultiMesh(PlantObject plant, List<PlantInstance> instances)
    {
        var multiMesh = new MultiMesh();
        multiMesh.TransformFormat = MultiMesh.TransformFormatEnum.Transform3D;
        multiMesh.InstanceCount = instances.Count;
        multiMesh.VisibleInstanceCount = -1; // Draw all instances

        multiMesh.SetMesh(plant.Mesh);
        int idx = 0;
        foreach (var instance in instances)
        {
            multiMesh.SetInstanceTransform(idx,new Transform3D(Basis.Identity,instance.WorldPosition));
            ++idx;
        }

        var multiMeshInstance = new MultiMeshInstance3D();
        multiMeshInstance.SetMultimesh(multiMesh);
        this.AddChild(multiMeshInstance);
        MultiMeshes.Add(multiMeshInstance);
    }
    
}