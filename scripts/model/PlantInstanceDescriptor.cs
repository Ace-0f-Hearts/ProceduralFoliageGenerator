using Godot;
using System;

namespace ProceduralFoliageGenerator.Model;

/// <summary>
/// Resource containing the necessary information about individual plants of the generated foliage.
/// </summary>
public class PlantInstanceDescriptor
{
    
    /// <summary>
    /// Global position of plant instance.
    /// </summary>
    private Vector3 _worldPosition = Vector3.Zero;
    
    /// <summary>
    /// Global scale of plant instance. Applied on all three axis.
    /// </summary>
    private float _scale = 1;

    public PlantInstanceDescriptor(Vector3 position, float scale)
    {
        _worldPosition = position;
        _scale = scale;
    }
}
