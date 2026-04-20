namespace ProceduralFoliageGenerator.Model;

public class Gaussian
{

    public Gaussian(float peak, float avg, float deviation)
    {
        this.Peak = peak;
        this.Avg = avg;
        this.Deviation = deviation;
    }
    public float Peak { get; set; }
    public float Avg { get; set; }
    public float Deviation { get; set; }
}