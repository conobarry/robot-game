using Godot;

public class MainCamera : Camera
{

    [Export]
    public float panMultiplier = 0.03f;

    [Export]
    public float orbitMultiplier = 0.03f;

    [Export]
    public float zoomStepping = 1f;


    private Cursor cursor;

    private Vector2 prevMouseLoc = Vector2.Zero;

    private Vector3 orbitOrigin = Vector3.Zero;

    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    public void _Init(Cursor cursor)
    {
        this.cursor = cursor;
    }

    private void UpdateMousePosition()
    {
        prevMouseLoc = GetViewport().GetMousePosition();
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("camera_pan"))
        {
            UpdateMousePosition();
        }
        if (Input.IsActionPressed("camera_pan"))
        {
            Pan();
        }

        if (Input.IsActionJustPressed("camera_orbit") && Input.IsActionPressed("camera_orbit_modifier"))
        {
            UpdateMousePosition();
            orbitOrigin = linePlaneIntersection(Vector3.Zero, Vector3.Up, GlobalTransform.origin, - Transform.basis.z);

        }
        if (Input.IsActionPressed("camera_orbit") && Input.IsActionPressed("camera_orbit_modifier"))
        {
            Orbit();
        }        

        // Vector2 mousePos = GetViewport().GetMousePosition();
        // Vector3 pos3d = ProjectPosition(mousePos, 4.0f);
        
        // pos3d = linePlaneIntersection(Vector3.Zero, Vector3.Up, Transform.origin, ProjectRayOrigin(mousePos));
        // Plane dropPlane = new Plane(new Vector3(0, 1, 0), 0);
        // pos3d = dropPlane.IntersectRay(ProjectRayOrigin(mousePos), ProjectRayNormal(mousePos)) ?? new Vector3(0, 1, 0);
        
        // pos3d = ViewportPointToGround(mousePos) ?? new Vector3(0, 1, 0);

        // position3D.Translation = pos3d;
        // GD.Print(position3D.Transform.origin);

        base._Process(delta);
    }

    public override void _PhysicsProcess(float delta)
    {        
        Highlight();

        base._PhysicsProcess(delta);
    }

    // While the _Process function works for panning and orbiting, it doesn't seem to like zooming
    //  so I've put those here. This is called whenever an input happens
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("camera_zoom_in"))
        {
            ZoomIn();
        } 
        else if (@event.IsActionPressed("camera_zoom_out"))
        {
            ZoomOut();
        }

        // tooltip.SetPosition(GetViewport().GetMousePosition());

        base._Input(@event);
    }

    private void Pan()
    {
        Vector2 currMouseLoc = GetViewport().GetMousePosition();
        Vector2 mouseDiff = prevMouseLoc - currMouseLoc;

        TranslateObjectLocal(new Vector3(mouseDiff.x * panMultiplier, 0, 0));
        TranslateObjectLocal(new Vector3(0, -mouseDiff.y * panMultiplier, 0));

        prevMouseLoc = currMouseLoc;
    }

    private void Orbit()
    {
        Vector2 currMouseLoc = GetViewport().GetMousePosition();
        Vector2 mouseDiff = prevMouseLoc - currMouseLoc;

        float angle = Mathf.Pi / 360;

        // This works pretty well but makes the camera zoom out as it orbits.
        //  Could be due to the angle? I just chose 1 degree cause it seemed right.
        Translate(new Vector3(orbitMultiplier * mouseDiff.x * (Mathf.Cos(angle) + orbitOrigin.x), 0, orbitMultiplier * mouseDiff.x * (Mathf.Sin(angle) + orbitOrigin.z)));
        LookAt(orbitOrigin, Vector3.Up);        

        prevMouseLoc = currMouseLoc;
    }

    private void Highlight()
    {
        Vector2 mousePos = GetViewport().GetMousePosition();

        Vector3 from = ProjectRayOrigin(mousePos);
        Vector3 to = from + ProjectRayNormal(mousePos) * 1000f;

        PhysicsDirectSpaceState spaceState = GetWorld().DirectSpaceState;
        Godot.Collections.Dictionary selection = spaceState.IntersectRay(from, to);
        
        if (selection.Count > 0)
        {
            Node collisionShape = (Node)selection["collider"];

            if (collisionShape is ICodeObject)
            {                
                cursor.TooltipUpper.Text = collisionShape.Name;
                ICodeObject codeobject = (ICodeObject) collisionShape;
                codeobject.Highlight();
            }
            else
            {
                cursor.TooltipUpper.Text = "";
            }

            // GD.Print(collisionShape.Name);
        }
    }

    // Finds the intersection point between a line and a plane
    private Vector3 linePlaneIntersection(Vector3 planePoint, Vector3 planeNormal, Vector3 linePoint, Vector3 lineDirection)
    {
        if (planeNormal.Dot(lineDirection.Normalized()) == 0)
        {
            return Vector3.Zero;
        }

        float t = (planeNormal.Dot(planePoint) - planeNormal.Dot(linePoint)) / planeNormal.Dot(lineDirection.Normalized());
        return linePoint + (lineDirection.Normalized() * t);
    }

    private void ZoomIn()
    {
        TranslateObjectLocal(new Vector3(0, 0, -zoomStepping));
    }

    private void ZoomOut()
    {
        TranslateObjectLocal(new Vector3(0, 0, zoomStepping));
    }

}
