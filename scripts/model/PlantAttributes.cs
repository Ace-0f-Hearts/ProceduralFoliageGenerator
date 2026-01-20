using System.Reflection;
using Godot;

namespace ProceduralFoliageGenerator.Model;

/// <summary>
/// Resource describing the simplified attributes of plant species used in the procedural foliage generation.
/// </summary>
public partial class PlantAttributes : Resource
{
    
    /// <summary>
    /// Parameterized constructor
    /// </summary>
    /// <param name="name"></param>
    /// <param name="growthRadius"></param>
    /// <param name="minimumElevationLevel"></param>
    /// <param name="maximumElevationLevel"></param>
    public PlantAttributes(string name, float growthRadius, float minimumElevationLevel, float maximumElevationLevel)
    {
        Name = name;
        GrowthRadius = growthRadius;
        MinimumElevationLevel = minimumElevationLevel;
        MaximumElevationLevel = maximumElevationLevel;
    }
    
    /// <summary>
    /// Name of the plant species.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Determines the necessary radius for the plant to grow and survive.
    /// Plants cannot have intersecting growth radii with each other.
    /// Measured in meters.
    /// </summary>
    public float GrowthRadius { get; init; }
    
    /// <summary>
    /// A simplified attribute derived from the real-life requirements of plantlife.
    /// Determines the minimum level of elevation necessary for the plant to grow and survive.
    /// Measured in meters.
    /// <br/>
    /// Must be smaller than <see cref="MaximumElevationLevel"/>
    /// </summary>
    public float MinimumElevationLevel { get; init; }
    
    /// <summary>
    /// A simplified attribute derived from the real-life requirements of plantlife.
    /// Determines the maximum level of elevation necessary for the plant to grow and survive.
    /// Measured in meters.
    /// <br/>
    /// Must be greater than <see cref="MinimumElevationLevel"/>
    /// </summary>
    public float MaximumElevationLevel { get; init; }
}