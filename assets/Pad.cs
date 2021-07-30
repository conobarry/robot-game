using Godot;

public class Pad : Spatial
{

    public bool IsTriggered { get; private set; }

    private MeshInstance mesh;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AddUserSignal("triggered");

        IsTriggered = false;

        mesh = (MeshInstance) FindNode("MeshInstance");

        Area area = (Area) FindNode("Area");
        area.Connect("body_entered", this, nameof(OnAreaEntered));
    }

    private void OnAreaEntered(Node body)
    {
        if (body is Cube)
        {
            IsTriggered = true;
            SpatialMaterial material = (SpatialMaterial) mesh.GetActiveMaterial(0);
            material.AlbedoColor = new Color("39ab3f");
            EmitSignal("triggered");
        }
    }

}
