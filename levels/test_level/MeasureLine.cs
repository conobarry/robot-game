using System.Collections.Generic;
using Godot;

public class MeasureLine : ImmediateGeometry
{

    private Vector3 measureStart;

    private ToolCamera camera;
    
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        camera = (ToolCamera) GetViewport().GetCamera();
        // GD.Print(GetParent());
        // camera = (Camera) GetParent<Camera>();
    }

    public override void _Process(float delta)
    {
        Vector2 mousePos = GetViewport().GetMousePosition();
        Vector3 mousePos3D = camera.ViewportPointToGround(mousePos).GetValueOrDefault();

        if (mousePos3D != default(Vector3))
        {
            if (Input.IsActionJustPressed("tools_measure"))
            {
                measureStart = mousePos3D;
            }
            if (Input.IsActionPressed("tools_measure"))
            {
                Clear();
                // DrawLine(measureStart, mousePos3D, Vector3.Up, 0.2f);
                // DrawSector(mousePos3D, 1, Mathf.Pi * 2, new Vector3(0, 0, 1), 20, Vector3.Up);
                DrawMeasureLine(measureStart, mousePos3D, Vector3.Up, 0.1f);
                
            }
            if (Input.IsActionJustReleased("tools_measure"))
            {
                Clear();
            }
        }       
        

        base._Process(delta);
    }

    private void DrawLine(Vector3 start, Vector3 end, Vector3 normal, float thickness)
    {
        List<Vector3> vertices = new List<Vector3>();

        Vector3 diff = end - start;
        Vector3 perpendicular = diff.Rotated(normal, Mathf.Pi / 2).Normalized() * thickness;

        Begin(Mesh.PrimitiveType.Triangles);        

        AddVertex(start - perpendicular);
        AddVertex(start + perpendicular);
        AddVertex(end - perpendicular);
        AddVertex(end + perpendicular);
        AddVertex(end - perpendicular);
        AddVertex(start + perpendicular);

        End();
        
    }

    private void DrawMeasureLine(Vector3 start, Vector3 end, Vector3 normal, float thickness)
    {
        DrawLine(start, end, normal, thickness);
        DrawSector(start, thickness, Mathf.Pi, start - end, 8, normal);
        DrawSector(end, thickness, Mathf.Pi, end - start, 8, normal);
    }

    private void DrawSector(Vector3 center, float radius, float angle, Vector3 direction, int segments, Vector3 normal)
    {
        Begin(Mesh.PrimitiveType.Triangles);

        direction = direction.IsNormalized() ? direction : direction.Normalized();

        float angleStep = angle / segments;

        // 1 = 0.5
        // 2 = 1
        // 3 = 1.5
        // 4 = 2

        Vector3 directionStep = direction.Rotated(normal, (angleStep * (segments / 2)));

        for (int i = 0; i < segments; i ++)
        {
            AddVertex(center);
            AddVertex(center + (directionStep * radius));
            directionStep = directionStep.Rotated(normal, -angleStep);
            AddVertex(center + (directionStep * radius));
        }

        End();
    }
}
