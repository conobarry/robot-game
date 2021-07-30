using Godot;

public class ControlButtons : CanvasLayer
{

    private Button OrthogonalButton { get; set; }

    private Button PerspectiveButton { get; set; }

    private Button ResetButton { get; set; }

    private Button TopDownButton { get; set; }

    private Button RobotButton { get; set; }

    private Transform oldCameraPos;

    private Godot.Camera.ProjectionEnum CameraProjection
    {
        get { return GetTree().Root.GetCamera().Projection; }
        set { GetTree().Root.GetCamera().Projection = value; }
    }

    private ButtonGroup cameraButtons;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        TopDownButton = (Button) FindNode(nameof(TopDownButton));
        TopDownButton.Connect("pressed", this, nameof(OnTopDownButtonClick));

        RobotButton = (Button) FindNode(nameof(RobotButton));
        RobotButton.Connect("pressed", this, nameof(OnRobotButtonClick));

        ResetButton = (Button) FindNode(nameof(ResetButton));
        ResetButton.Connect("pressed", this, nameof(OnResetButtonClick));
            
        OrthogonalButton = (Button) FindNode(nameof(OrthogonalButton));
        PerspectiveButton = (Button) FindNode(nameof(PerspectiveButton));

        // Adding these buttons to a group means they will automatically
        //  toggle if the other is clicked.
        cameraButtons = new ButtonGroup();
        OrthogonalButton.Group = cameraButtons;
        PerspectiveButton.Group = cameraButtons;

        if ( CameraProjection == Godot.Camera.ProjectionEnum.Perspective )
        {
            PerspectiveButton.Pressed = true;
        }
        else
        {
            OrthogonalButton.Pressed = true;
        }

        OrthogonalButton.Connect("pressed", this, nameof(OnOrthogonalButtonClick));
        PerspectiveButton.Connect("pressed", this, nameof(OnPerspectiveButtonClick));
    }

    private void OnOrthogonalButtonClick()
    {
        CameraProjection = Godot.Camera.ProjectionEnum.Orthogonal;
    }

    private void OnPerspectiveButtonClick()
    {
        CameraProjection = Godot.Camera.ProjectionEnum.Perspective;
    }

    private void OnResetButtonClick()
    {
        GetTree().ChangeScene("res://levels/test_level/test_level.tscn");
    }

    private void OnTopDownButtonClick()
    {
        // oldCameraPos = GetTree().Root.GetCamera().Transform;
        // Transform topDown = new Transform();
        // topDown = topDown.Rotated(Vector3.Left, -Mathf.Pi);
        // topDown = topDown.Translated(new Vector3(0, 25, 0));
        MainCamera mainCamera = (MainCamera) GetTree().Root.GetCamera();
        mainCamera.MoveToTopDown();
    }

    private void OnRobotButtonClick()
    {
        MainCamera mainCamera = (MainCamera) GetTree().Root.GetCamera();

        if (RobotButton.Pressed)
        {
            mainCamera.FollowRobot = true;
        }
        else
        {
            mainCamera.FollowRobot = false;
        }
    }

}
