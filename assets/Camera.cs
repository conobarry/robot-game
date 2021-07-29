using Godot;

public class Camera : Godot.Camera
{
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public Vector3? ViewportPointToGround(Vector2 viewportPoint)
    {
        Plane groundPlane = new Plane(0, 1, 0, 0);
        Vector3 origin = ProjectRayOrigin(viewportPoint);
        Vector3 normal = ProjectRayNormal(viewportPoint);
        Vector3? groundPoint = groundPlane.IntersectRay(origin, normal);
        return groundPoint;
    }

    public Vector2 GroundPointToViewport(Vector3 groundPoint)
    {
        return UnprojectPosition(groundPoint);
    }

}
