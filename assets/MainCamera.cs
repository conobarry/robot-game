using Godot;

public class MainCamera : Camera
{

    [Export]
    public float panMultiplier = 0.03f;

    [Export]
    public float orbitMultiplier = 0.03f;

    [Export]
    public float zoomStepping = 1f;

    public bool FollowRobot
    {
        get
        {
            return isFollowingRobot;
        }
        set
        {
            if (value)
            {
                oldTransform = Transform;
                Projection = ProjectionEnum.Perspective;
            }
            else
            {
                Transform = oldTransform;
            }
            
            isFollowingRobot = value;
        }
    }

    private bool isFollowingRobot = false;

    private Cursor cursor;

    private Robot robot;

    private Transform oldTransform;

    private Spatial level;

    private Vector2 prevMouseLoc = Vector2.Zero;

    private Vector3 orbitOrigin = Vector3.Zero;

    private Position3D topDownPosition;

    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        topDownPosition = (Position3D) GetParent().FindNode("TopDownPosition");
    }

    public void _Init(Cursor cursor, Robot robot)
    {
        this.cursor = cursor;
        this.robot = robot;
    }

    private void UpdateMousePosition()
    {
        prevMouseLoc = GetViewport().GetMousePosition();
    }

    public override void _Process(float delta)
    {
        if (! FollowRobot)
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
        }

        base._Process(delta);
    }

    public override void _PhysicsProcess(float delta)
    {        
        if (FollowRobot)
        {
            Transform = robot.CameraPosition.GlobalTransform;
        }

        base._PhysicsProcess(delta);
    }

    // While the _Process function works for panning and orbiting, it doesn't seem to like zooming
    //  so I've put those here. This is called whenever an input happens
    public override void _Input(InputEvent @event)
    {
        if (! FollowRobot)
        {
            if (@event.IsActionPressed("camera_zoom_in"))
            {
                ZoomIn();
            } 
            else if (@event.IsActionPressed("camera_zoom_out"))
            {
                ZoomOut();
            }
        }

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
        // orbitOrigin = Projection == ProjectionEnum.Perspective ? orbitOrigin : Vector3.Zero;

        Vector2 currMouseLoc = GetViewport().GetMousePosition();
        Vector2 mouseDiff = prevMouseLoc - currMouseLoc;

        float angle = Mathf.Pi / 360;        

        if (Projection == ProjectionEnum.Perspective)
        {            
            // This works pretty well but makes the camera zoom out as it orbits.
            Translate(new Vector3(orbitMultiplier * mouseDiff.x * (Mathf.Cos(angle) + orbitOrigin.x), 0, orbitMultiplier * mouseDiff.x * (Mathf.Sin(angle) + orbitOrigin.z)));
            LookAt(orbitOrigin, Vector3.Up);   
        }
        else if (Projection == ProjectionEnum.Orthogonal)
        {
            // GlobalRotate(Vector3.Up, angle);
            Translate(new Vector3(orbitMultiplier * mouseDiff.x * (Mathf.Cos(angle) + orbitOrigin.x), 0, orbitMultiplier * mouseDiff.x * (Mathf.Sin(angle) + orbitOrigin.z)));
            LookAt(orbitOrigin, Vector3.Up);  
        }

        prevMouseLoc = currMouseLoc;
        
    }

    public void MoveToTopDown()
    {
        Transform = topDownPosition.Transform;
    }


    // private void Highlight()
    // {
    //     Vector2 mousePos = GetViewport().GetMousePosition();

    //     Vector3 from = ProjectRayOrigin(mousePos);
    //     Vector3 to = from + ProjectRayNormal(mousePos) * 1000f;

    //     PhysicsDirectSpaceState spaceState = GetWorld().DirectSpaceState;
    //     Godot.Collections.Dictionary selection = spaceState.IntersectRay(from, to);
        
    //     if (selection.Count > 0)
    //     {
    //         Node collisionShape = (Node)selection["collider"];

    //         if (collisionShape is ICodeObject)
    //         {                
    //             cursor.TooltipUpper.Text = collisionShape.Name;
    //             ICodeObject codeobject = (ICodeObject) collisionShape;
    //             codeobject.Highlight();
    //         }
    //         else
    //         {
    //             cursor.TooltipUpper.Text = "";
    //         }

    //         // GD.Print(collisionShape.Name);
    //     }
    // }

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
        if (Projection == ProjectionEnum.Perspective)
        {
            TranslateObjectLocal(new Vector3(0, 0, -zoomStepping));
        }
        else if (Projection == ProjectionEnum.Orthogonal)
        {
            Size -= zoomStepping;
        }        
    }

    private void ZoomOut()
    {
        if (Projection == ProjectionEnum.Perspective)
        {
            TranslateObjectLocal(new Vector3(0, 0, zoomStepping));
        }
        else if (Projection == ProjectionEnum.Orthogonal)
        {
            Size += zoomStepping;
        }  
    }

}
