using System.Collections.Generic;
using Godot;

namespace ProceduralFoliageGenerator.Model;

public class PlantInstancesParser
{

    public static List<PlantInstance> Parse(string jsonContent)
    {
        List<PlantInstance> instances = new List<PlantInstance>();

        var attr = Json.ParseString(jsonContent);
        var array = attr.AsGodotArray();

        foreach (var item in array)
        {
            var rawInstance = item.AsGodotDictionary();
            
            var id = rawInstance["id"].AsInt32();
            var scale = rawInstance["scale"].AsSingle();
            var x = rawInstance["x"].AsSingle();
            var y = rawInstance["y"].AsSingle();

            var instance = new PlantInstance(id, scale, new Vector3(x,0,y));
            
            instances.Add(instance);
        }
        
        return instances;
    }
}