using Godot;
using ProceduralFoliageGenerator.Model;

namespace ProceduralFoliageGenerator.scripts.model;

public class MapDataParser
{
    public static MapData Parse(string content)
    {
        var json = Json.ParseString(content);
        var mapDataDict = json.AsGodotDictionary();

        MapData data = new();

        if (!mapDataDict.ContainsKey("height") || !mapDataDict.ContainsKey("width") ||
            !mapDataDict.ContainsKey("h_offset") || !mapDataDict.ContainsKey("v_offset"))
        {
            GD.PrintErr("Illformed map data");
            return null;
        }

        data.Height = mapDataDict["height"].AsSingle();
        data.Width = mapDataDict["width"].AsSingle();
        data.HorizontalOffset = mapDataDict["h_offset"].AsSingle();
        data.VerticalOffset = mapDataDict["v_offset"].AsSingle();

        return data;
    }
}