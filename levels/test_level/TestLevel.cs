using Godot;

public class TestLevel : Spatial
{
	
	private Sprite cursor;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScriptEditor scriptEditor = (ScriptEditor)FindNode("ScriptEditor");
				
		Robot robot = (Robot)FindNode("Robot");		

		cursor = (Sprite)FindNode("Cursor");

		Resource plus = ResourceLoader.Load("res://plus.png");
		Input.SetCustomMouseCursor(plus);

		Input.SetMouseMode(Input.MouseMode.Hidden);

		GD.Print(robot.Name);
				
		scriptEditor._Init(robot);
	}

    public override void _Process(float delta)
    {
		cursor.Position = GetViewport().GetMousePosition();

        base._Process(delta);
    }

}
