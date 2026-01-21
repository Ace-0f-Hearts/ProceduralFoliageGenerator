using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;


namespace ProceduralFoliageGenerator.Model;
/// <summary>
/// Parser class responsible for processing a JSON file containing the data about plant species and the generated foliage.
/// </summary>
public class FoliageDescriptorParser
{
    public System.Collections.Generic.Dictionary<String, List<PlantInstanceDescriptor>> Parse(string json)
    {
        System.Collections.Generic.Dictionary<String, List<PlantInstanceDescriptor>> result  = new();
        
        var v = Json.ParseString(json);
        var dict = v.AsGodotDictionary();

        foreach (var key in dict.Keys )
        {
            var keyStr = key.AsString();
            var instanceDescriptors = new List<PlantInstanceDescriptor>();
            
            var instances = dict[keyStr];

            foreach (var instanceVar in instances.AsGodotArray())
            {
                var instance = instanceVar.AsGodotArray();
                
                if (instance.Count != 4)
                    throw new Exception("Incorrect number of items in array");
                
                var position = new Vector3(instance[0].AsSingle(), instance[1].AsSingle(), instance[2].AsSingle());
                instanceDescriptors.Add(new PlantInstanceDescriptor(position, instance[3].AsSingle()));
            }
            result.Add(keyStr, instanceDescriptors);
        }
        return result;
    }
    
    //TODO: Do the hash/key check outside through the name of the fiels
    
}
