namespace ProceduralFoliageGenerator.Model;

/// <summary>
/// Data about map dimensions
/// </summary>
public class MapData
{
    public float Width { get; set; } = 1000;
    public float Height { get; set; } = 1000;
    

    public float HorizontalOffset { get; set; } = 0;
    public float VerticalOffset { get; set; } = 0;

    public float HeightScale { get; set; } = 1.0f;
    public float Scaling { get; set; } = 100;
}