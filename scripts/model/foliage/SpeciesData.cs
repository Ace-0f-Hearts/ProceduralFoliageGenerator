using System.Collections.Generic;

namespace ProceduralFoliageGenerator.Model;

public record SpeciesData
{
    public SpeciesData(PlantAttributes plantAttributes, PlantObject plantObject, List<PlantInstance> instances)
    {
        PlantObject = plantObject;
        PlantAttributes = plantAttributes;
        Instances = instances;
    }

    public List<PlantInstance> Instances { get; set; }
    public PlantObject PlantObject { get; set; }
    public PlantAttributes PlantAttributes { get; set; }
}