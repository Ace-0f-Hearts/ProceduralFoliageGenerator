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
            var instances = new List<PlantInstanceDescriptor>();
            
            var p_inst = dict[keyStr];
            var array = p_inst.AsGodotArray<Array<Single>>();
            foreach (var item in array)
            {
                if (item.Count != 4)
                    throw new Exception("Incorrect number of items in array");
                
                var position = new Vector3(item[0], item[1], item[2]);
                instances.Add(new PlantInstanceDescriptor(position, item[3]));
            }
            result.Add(keyStr, instances);
        }
        return result;
    }
    
    //TODO: Do the hash/key check outside through the name of the fiels
    
}
