using System.Collections.Generic;
using Godot;

namespace ProceduralFoliageGenerator.Model;

public class PlantInstancesParser
{
    public static List<PlantInstance> Parse(string jsonContent)
    {
        var instances = new List<PlantInstance>();

        var attr = Json.ParseString(jsonContent);
        var array = attr.AsGodotArray();

        foreach (var item in array)
        {
            var rawInstance = item.AsGodotDictionary();

            if (!rawInstance.ContainsKey("id") || !rawInstance.ContainsKey("scale") || !rawInstance.ContainsKey("x") ||
                !rawInstance.ContainsKey("y"))
            {
                GD.Print("Ill-formed list of instances");
                return new List<PlantInstance>();
            }

            var id = rawInstance["id"].AsInt32();
            var scale = rawInstance["scale"].AsSingle();
            var x = rawInstance["x"].AsSingle();
            var y = rawInstance["y"].AsSingle();

            var instance = new PlantInstance(id, scale, new Vector3(x, 0, y));

            instances.Add(instance);
        }

        return instances;
    }
}