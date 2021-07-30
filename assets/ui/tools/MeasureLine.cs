using System.Collections.Generic;
using System.Linq;
using Godot;

public class MeasureLine : ImmediateGeometry
{

    [Export]
    public float Thickness = 0.1f;

    private Vector3? measureStart = null;

    private ToolCamera toolCamera;

    private Camera mainCamera; 

    private Cursor cursor;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        toolCamera = (ToolCamera) GetViewport().GetCamera();
        mainCamera = (Camera) GetTree().Root.GetCamera();
    }

    public void _Init(Cursor cursor)
    {
        this.cursor = cursor;
    }

    public override void _Process(float delta)
    {
        

        base._Process(delta);
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector2 mousePos = GetViewport().GetMousePosition();
        Vector3? mousePos3D = toolCamera.ViewportPointToGround(mousePos);        

        if (mousePos3D != null)
        {
            Vector3 mousePos3DValue = mousePos3D.Value;

            if (Input.IsActionJustPressed("tools_measure"))
            {
                Vector3 from = mainCamera.ProjectRayOrigin(mousePos);
                Vector3 to = from + mainCamera.ProjectRayNormal(mousePos) * 1000f;              

                PhysicsDirectSpaceState spaceState = mainCamera.GetWorld().DirectSpaceState;
                Godot.Collections.Dictionary selection = spaceState.IntersectRay(from, to);

                if (selection.Count > 0)
                {
                    Spatial collisionShape = (Spatial) selection["collider"];

                    bool isCodeObject = collisionShape.GetType().GetInterfaces().Any(x => 
                        x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICodeObject<>));

                    if (isCodeObject)
                    {
                        measureStart = collisionShape.Transform.origin;
                    }
                    else
                    {
                        measureStart = mousePos3DValue;
                    }
                }
                else
                {
                    measureStart = mousePos3DValue;
                }

            }
            if (Input.IsActionPressed("tools_measure"))
            {
                if (measureStart != null)
                {
                    Vector3 measureStartValue = measureStart.Value;
                    float length = (mousePos3DValue - measureStartValue).Length();
                    Clear();
                    DrawMeasureLine(measureStartValue, mousePos3DValue, Vector3.Up, Thickness);   
                    UpdateCursor(length);
                }
            }
            if (Input.IsActionJustReleased("tools_measure"))
            {
                Clear();
                measureStart = null;
                cursor.SetDefault();
                cursor.TooltipLower.Text = "";
            }
        }

        base._PhysicsProcess(delta);
    }

    private void UpdateCursor(float length)
    {
        cursor.SetCursor(CursorType.Ruler);
        cursor.TooltipLower.Text = $"{length.ToString("F2")}m";
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

    private void DrawSector(Vector3 center, float radius, float angle, Vector3 direction, int segments, Vector3 normal)
    {
        Begin(Mesh.PrimitiveType.Triangles);

        direction = direction.IsNormalized() ? direction : direction.Normalized();

        float angleStep = angle / segments;

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
    
    private void DrawMeasureLine(Vector3 start, Vector3 end, Vector3 normal, float thickness)
    {
        DrawLine(start, end, normal, thickness);
        DrawSector(start, thickness, Mathf.Pi, start - end, 8, normal);
        DrawSector(end, thickness, Mathf.Pi, end - start, 8, normal);
    }
}
