using System.Collections.Generic;
using System.Linq;
using Godot;

namespace ProceduralFoliageGenerator.Model;

public class Foliage
{
    public Foliage(List<SpeciesData> speciesData)
    {
        SpeciesData = speciesData;
    }

    private List<SpeciesData> SpeciesData { get; }

    public List<string> GetSpeciesNames()
    {
        return SpeciesData.Select(data => { return data.PlantAttributes.Name; }).ToList();
    }

    public List<int> GetSpeciesIds()
    {
        return SpeciesData.Select(data => data.PlantAttributes.Id).ToList();
    }

    public List<PlantInstance> GetInstances()
    {
        return SpeciesData.SelectMany(data => data.Instances).ToList();
    }

    public Dictionary<PlantObject, List<PlantInstance>> GetInstancesPerObjects()
    {
        return SpeciesData.ToDictionary(kvp => kvp.PlantObject, kvp => kvp.Instances);
    }

    public Dictionary<PlantAttributes, List<PlantInstance>> GetInstancesPerAttributes()
    {
        return SpeciesData.ToDictionary(kvp => kvp.PlantAttributes, kvp => kvp.Instances);
    }

    public void SetObjectMeshData(int nth, Mesh mesh)
    {
        SpeciesData[nth].PlantObject.TrunkMesh = mesh;
    }

    public void SetObjectData(int nth, PlantObject obj)
    {
        SpeciesData[nth].PlantObject = obj;
    }
}