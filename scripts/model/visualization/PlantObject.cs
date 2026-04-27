using Godot;
using System;

[GlobalClass]
public partial class PlantObject : Resource
{

    [Export] public string Name { get; set; }
    [Export] public Mesh TrunkMesh { get; set; }
    [Export] public Mesh FoliageMesh { get; set; }
    
    [Export] public Texture2D PlantTexture { get; set; }

    public PlantObject()
    {
        
        TrunkMesh = new BoxMesh();
        FoliageMesh = new BoxMesh();
        Name = "DEFAULT";
    }
    public PlantObject(string name, Mesh trunkMesh)
    {
        Name = name;
        TrunkMesh = trunkMesh;
    }
    

    

}
