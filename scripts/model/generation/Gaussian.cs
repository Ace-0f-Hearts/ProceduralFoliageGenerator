namespace ProceduralFoliageGenerator.Model;

public class Gaussian
{
    public Gaussian(float peak, float avg, float deviation)
    {
        Peak = peak;
        Avg = avg;
        Deviation = deviation;
    }

    public float Peak { get; set; }
    public float Avg { get; set; }
    public float Deviation { get; set; }
}