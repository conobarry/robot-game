using Godot;

public class ToolCamera : Godot.Camera
{
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    public override void _Process(float delta)
    {
        Transform = this.GetTree().Root.GetCamera().Transform;

        base._Process(delta);
    }

    public Vector3? ViewportPointToGround(Vector2 viewportPoint)
    {
        Plane groundPlane = new Plane(0, 1, 0, 0);
        Vector3 origin = ProjectRayOrigin(viewportPoint);
        Vector3 normal = ProjectRayNormal(viewportPoint);
        return groundPlane.IntersectRay(origin, normal);
    }

    public Vector2 GroundPointToViewport(Vector3 groundPoint)
    {
        return UnprojectPosition(groundPoint);
    }    
}
