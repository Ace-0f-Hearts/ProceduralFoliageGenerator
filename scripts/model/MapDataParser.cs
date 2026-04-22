using System;
using Godot;
using Godot.Collections;
using ProceduralFoliageGenerator.Model;

namespace ProceduralFoliageGenerator.scripts.model;

public class MapDataParser
{
    public static MapData Parse(String content)
    {
        var json = Json.ParseString(content);
        var mapDataDict = json.AsGodotDictionary();
        
        MapData data = new();
        
        data.Height = (mapDataDict["height"].AsSingle());
        data.Width = (mapDataDict["width"].AsSingle());
        data.HorizontalOffset = (mapDataDict["h_offset"].AsSingle());
        data.VerticalOffset = mapDataDict["v_offset"].AsSingle();
        
        return data;
    }
}