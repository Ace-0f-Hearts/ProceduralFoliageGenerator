using Godot;
using System;

public partial class PlantObject : Resource
{

    [Export] public string Name { get; set; }
    [Export] public Mesh Mesh { get; set; }

    public PlantObject(string name, Mesh mesh)
    {
        Name = name;
        Mesh = mesh;
    }
    
    public static PlantObject Default()
    {
        Mesh mesh = new CapsuleMesh();
        
        return new PlantObject("DEFAULT",mesh);
    }

}
