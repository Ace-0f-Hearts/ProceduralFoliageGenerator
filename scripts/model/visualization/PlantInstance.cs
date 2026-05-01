using Vector2 = Godot.Vector2;
using Vector3 = Godot.Vector3;

namespace ProceduralFoliageGenerator.Model;

/// <summary>
///     Resource containing the necessary information about individual plants of the generated foliage.
/// </summary>
public class PlantInstance
{
    private Vector3 _worldPosition;


    public PlantInstance(int id, float scale, Vector3 position)
    {
        Id = id;
        Scale = scale;
        WorldPosition = position;
    }

    /// <summary>
    ///     Identifies which plant species this particular instance belongs to.
    /// </summary>
    public int Id { get; private set; } = -1;

    /// <summary>
    ///     Global scale of plant instance. Applied on all three axis.
    /// </summary>
    public float Scale { get; private set; } = 1;

    /// <summary>
    ///     Global position of plant instance.
    /// </summary>
    public Vector3 WorldPosition
    {
        get => _worldPosition;
        private set => _worldPosition = value;
    }

    public void SetWorldPosition(Vector3 worldPosition)
    {
        WorldPosition = worldPosition;
    }

    public void SetMapCoordinate(Vector2 coordinate)
    {
        _worldPosition.X = coordinate.X;
        _worldPosition.Z = coordinate.Y;
    }

    public void SetElevation(float elevation)
    {
        _worldPosition.Y = elevation;
    }

    public void SetScale(float scale)
    {
        Scale = scale;
    }

    public void SetSpeciesId(int speciesId)
    {
        Id = speciesId;
    }
}