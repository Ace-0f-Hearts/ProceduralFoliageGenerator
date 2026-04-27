namespace ProceduralFoliageGenerator.Model;

public class RangeValue
{
    public RangeValue(float min, float max)
    {
        this.Min = min;
        this.Max = max;
    }
    public float Max { get; set; }
    public float Min { get; set; }
}