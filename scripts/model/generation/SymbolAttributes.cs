using Godot;

namespace ProceduralFoliageGenerator.Model;

public partial class SymbolAttributes : Resource
{
    public SymbolAttributes()
    {
    }

    public SymbolAttributes(int id, short flags, float radius)
    {
        Id = id;
        Flags = flags;
        Radius = radius;
    }

    public int Id { get; set; }
    public short Flags { get; set; }
    public float Radius { get; set; }
}