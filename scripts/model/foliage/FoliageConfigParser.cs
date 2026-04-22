using System;
using Godot;
using ProceduralFoliageGenerator.Model;

namespace ProceduralFoliageGenerator.scripts.model;

public class FoliageConfigParser
{
    public static FoliageConfig Parse(String content)
    {
        FoliageConfig config = new ();
        var json = Json.ParseString(content);
        var jsonObject = json.AsGodotDictionary();
        
        config.PathToSpeciesAttributes = jsonObject["species"].AsString();
        config.PathToInstances = jsonObject["instances"].AsString();
        config.PathToMapData = jsonObject["map_data"].AsString();
        config.PathToHeightMap =  jsonObject["height_map"].AsString();
        
        return config;
    } 
}