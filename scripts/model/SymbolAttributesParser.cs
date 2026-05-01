using System.Collections.Generic;
using Godot;
using ProceduralFoliageGenerator.Model;

namespace ProceduralFoliageGenerator.scripts.model;

public class SymbolAttributesParser
{
    public static List<SymbolAttributes> Parse(string jsonContent)
    {
        List<SymbolAttributes> symbolAttributesList = new();

        var attr = Json.ParseString(jsonContent);
        var array = attr.AsGodotArray();

        foreach (var item in array)
        {
            var symbolAttr = item.AsGodotDictionary();

            if (!symbolAttr.ContainsKey("id") || !symbolAttr.ContainsKey("flags") ||
                !symbolAttr.ContainsKey("radius"))
            {
                GD.Print("Ill-formed symbol attributes list!");
                return new List<SymbolAttributes>();
            }

            var id = symbolAttr["id"].AsInt32();
            var flags = symbolAttr["flags"].AsInt16();
            var radius = symbolAttr["radius"].AsSingle();

            var res = new SymbolAttributes(id, flags, radius);
            symbolAttributesList.Add(res);
        }

        return symbolAttributesList;
    }
}