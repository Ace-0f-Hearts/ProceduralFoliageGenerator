namespace ProceduralFoliageGenerator.Model;

public class RangeValue
{
    /// <summary>
    ///     Paramaterized constructor
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public RangeValue(float min, float max)
    {
        Min = min;
        Max = max;
    }

    public float Max { get; set; }
    public float Min { get; set; }
}