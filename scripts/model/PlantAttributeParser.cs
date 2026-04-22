using System.Collections.Generic;
using System.Text.Json.Serialization;
using Godot;
using Godot.Collections;

namespace ProceduralFoliageGenerator.Model;

public class PlantAttributeParser
{

    public static Range ParseRange(Array range)
    {
        var min = range[0].AsSingle();
        var max = range[0].AsSingle();
        return new Range(min, max);
    }

    public static Gaussian ParseGaussian(Array gaussian)
    {
        var peak = gaussian[0].AsSingle();
        var avg = gaussian[1].AsSingle();
        var deviation = gaussian[2].AsSingle();
        return new Gaussian(peak, avg, deviation);
    }
    
    public static List<PlantAttributes> Parse(string jsonContent)
    {
        List<PlantAttributes> plantAttributesList = new();
        
        var attr = Json.ParseString(jsonContent) ;
        var array = attr.AsGodotArray();

        foreach (var item in array)
        {
            var plantAttr = item.AsGodotDictionary();
            
            var id = plantAttr["id"].AsInt32();
            var name = plantAttr["name"].AsStringName();
            var growthRadius = ParseRange(plantAttr["growth_radius"].AsGodotArray());
            var elevation  = ParseGaussian(plantAttr["elevation"].AsGodotArray());
            var slope = ParseGaussian(plantAttr["slope"].AsGodotArray());
            
            var res = new PlantAttributes(id,name, growthRadius, elevation, slope);
            plantAttributesList.Add(res);
        }

        

        return plantAttributesList;
    }
}