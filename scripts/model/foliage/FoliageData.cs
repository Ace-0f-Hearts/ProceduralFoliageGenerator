using System.Collections.Generic;
using System.Linq;
using Godot;

namespace ProceduralFoliageGenerator.Model;

/// <summary>
///     Responsible for the intermediate data found in the foliage config files
/// </summary>
public class FoliageData
{
    private Image _heightMap;
    private MapData _mapData;
    private List<PlantInstance> _plantInstances = new();

    public Image HeightMap
    {
        get => _heightMap;
        set
        {
            _heightMap = value;
            UpdateElevationOfInstances();
        }
    }

    public Image MapTexture { get; set; }

    public MapData MapData
    {
        get => _mapData;
        set
        {
            _mapData = value;
            FixInstancesOffset();
            UpdateElevationOfInstances();
        }
    }


    public List<(string, int)> GetNumberOfInstancesPerSpecies =>
        GetPlantInstancesPerSpecies().Select(i => (i.Key.Name, i.Value.Count)).ToList();

    public List<PlantInstance> PlantInstances
    {
        get => _plantInstances;
        set
        {
            _plantInstances = value;
            FixInstancesOffset();
            UpdateElevationOfInstances();
        }
    }

    public List<PlantAttributes> PlantAttributes { get; set; } = new();

    public Dictionary<PlantAttributes, List<PlantInstance>> GetPlantInstancesPerSpecies()
    {
        var result = new Dictionary<PlantAttributes, List<PlantInstance>>();

        foreach (var attr in PlantAttributes)
        {
            var instances = PlantInstances.FindAll(instance => { return instance.Id == attr.Id; });
            result.Add(attr, instances);
        }

        return result;
    }

    public void Clear()
    {
        _plantInstances?.Clear();
        PlantAttributes?.Clear();
        _mapData = null;
    }


    public bool ReadyToUpdateElevation()
    {
        return HeightMap is not null && !HeightMap.IsEmpty() && MapData is not null && PlantInstances.Count > 0;
    }

    public bool ReadyToFixInstancesOffset()
    {
        return MapData is not null && PlantInstances.Count > 0;
    }

    public void FixInstancesOffset()
    {
        if (!ReadyToFixInstancesOffset()) return;

        var offset = new Vector3(MapData.HorizontalOffset, 0, MapData.VerticalOffset);
        foreach (var instance in PlantInstances)
        {
            var pos = instance.WorldPosition;
            pos -= offset;

            instance.SetWorldPosition(pos);
        }
    }

    public void UpdateElevationOfInstances()
    {
        if (!ReadyToUpdateElevation())
            return;

        foreach (var instance in PlantInstances)
        {
            var x = instance.WorldPosition.X / _mapData.Width;
            var z = instance.WorldPosition.Z / _mapData.Height;

            var elevation = 0.025f +
                            HeightMap.GetPixel((int)(x * HeightMap.GetWidth()), (int)(z * HeightMap.GetHeight())).R *
                            MapData.Scaling * MapData.HeightScale;

            instance.SetElevation(elevation);
        }
    }
}