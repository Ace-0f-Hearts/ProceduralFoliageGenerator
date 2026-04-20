namespace ProceduralFoliageGenerator.Model;

public class Range
{
    public Range(float min, float max)
    {
        this.Min = min;
        this.Max = max;
    }
    public float Max { get; set; }
    public float Min { get; set; }
}