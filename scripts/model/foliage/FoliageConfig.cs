namespace ProceduralFoliageGenerator.Model;

public class FoliageConfig
{
    public string PathToSpeciesAttributes { get; set; }
    public string PathToMapData { get; set; }
    public string PathToHeightMap { get; set; }
    public string PathToInstances { get; set; }
    public string PathToMapTexture { get; set; }

    public bool IsReady()
    {
        return !string.IsNullOrEmpty(PathToMapData) && !string.IsNullOrEmpty(PathToInstances) &&
               !string.IsNullOrEmpty(PathToSpeciesAttributes) && !string.IsNullOrEmpty(PathToHeightMap);
    }

    public void Clear()
    {
        PathToSpeciesAttributes = string.Empty;
        PathToMapData = string.Empty;
        PathToInstances = string.Empty;
        PathToMapTexture = string.Empty;
        PathToHeightMap = string.Empty;
    }
}