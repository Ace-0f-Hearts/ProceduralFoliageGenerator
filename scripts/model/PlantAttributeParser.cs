using System.Collections.Generic;
using System.Text.Json.Serialization;
using Godot;

namespace ProceduralFoliageGenerator.Model;

public class PlantAttributeParser
{
    public PlantAttributeParser() {}

    
    
    public List<PlantAttributes> Parse(string jsonContent)
    {
        List<PlantAttributes> plantAttributesList = new();
        
        var attr = Json.ParseString(jsonContent) ;
        var array = attr.AsGodotArray();

        foreach (var item in array)
        {
            var plantAttr = item.AsGodotDictionary();
            
            var name = plantAttr["name"].AsStringName();
            var growthRadius = plantAttr["growthRadius"].AsSingle();
            var minElevation  = plantAttr["minElevation"].AsSingle();
            var maxElevation = plantAttr["maxElevation"].AsSingle();
            
            var res = new PlantAttributes(name, growthRadius, minElevation, maxElevation);
            plantAttributesList.Add(res);
        }

        

        return plantAttributesList;
    }
}