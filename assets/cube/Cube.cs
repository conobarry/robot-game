using Godot;
using System;

public class Cube : RigidBody
{

    [Export]
    public Color Color
    {
        get { return Material.AlbedoColor; }
        set { Material.AlbedoColor = value; }
    }

    private SpatialMaterial Material
    {
        get
        {
            Material material = mesh.GetActiveMaterial(0);
            return (material is SpatialMaterial) ? (SpatialMaterial) material : null;
        }
    }

    private MeshInstance mesh;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {   
        mesh = (MeshInstance) FindNode("MeshInstance");
    }

}
