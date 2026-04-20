using Godot;
using System;
using Godot.Collections;
using Array = Godot.Collections.Array;

namespace ProceduralFoliageGenerator.ViewModel
{
    public partial class FoliageMultiMesh : Node3D
    {
        private float[] _instancePositions;
        
        public Rid MultiMeshRid { get; private set; }
        
        public float[] InstancePositions { get; set; }
        
        [Export]
        public Mesh Mesh { get; set; }
        
        
        public override void _Ready()
        {
            MultiMeshRid = RenderingServer.MultimeshCreate();
            
            var scenario = GetWorld3D().Scenario;
            
            RenderingServer.InstanceSetBase(MultiMeshRid, scenario);
            RenderingServer.MultimeshSetMesh(MultiMeshRid, Mesh.GetRid());
            
            base._Ready();
        }

        public void SetInstanceTransforms()
        {   
            RenderingServer.MultimeshSetBuffer(MultiMeshRid, InstancePositions);
        }

        public override void _ExitTree()
        {
            RenderingServer.FreeRid(MultiMeshRid);
            base._ExitTree();
        }
    }
    
}
