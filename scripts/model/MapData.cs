namespace ProceduralFoliageGenerator.Model;

/// <summary>
/// Data about map dimensions
/// </summary>
public class MapData
{
    public float Width { get; set; } = 10;
    public float Height { get; set; } = 10;

    public float MaxHeight { get; set; } = 10;
    public float MinHeight { get; set; } = 0;
}