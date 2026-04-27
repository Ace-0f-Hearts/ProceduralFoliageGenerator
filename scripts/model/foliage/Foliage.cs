using System.Collections.Generic;
using System.Linq;
using Godot;

namespace ProceduralFoliageGenerator.Model;

public class Foliage
{
    private List<SpeciesData> SpeciesData { get; set; }

    public Foliage(List<SpeciesData> speciesData)
    {
        SpeciesData = speciesData;
    }

    public List<string> GetSpeciesNames() => SpeciesData.Select(data => { return data.PlantAttributes.Name; }).ToList();
    public List<int> GetSpeciesIds() => SpeciesData.Select(data => data.PlantAttributes.Id).ToList();
    public List<PlantInstance> GetInstances() => SpeciesData.SelectMany(data => data.Instances).ToList();
    
    public Dictionary<PlantObject, List<PlantInstance>> GetInstancesPerObjects() => SpeciesData.ToDictionary(kvp => kvp.PlantObject, kvp => kvp.Instances);
    
    public Dictionary<PlantAttributes, List<PlantInstance>> GetInstancesPerAttributes() => SpeciesData.ToDictionary(kvp => kvp.PlantAttributes, kvp => kvp.Instances);
    
    public void SetObjectMeshData(int nth,Mesh mesh) => SpeciesData[nth].PlantObject.TrunkMesh = mesh;
    public void SetObjectData(int nth, PlantObject obj) => SpeciesData[nth].PlantObject = obj;
}