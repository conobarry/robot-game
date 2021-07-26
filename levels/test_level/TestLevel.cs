using Godot;

public class TestLevel : Spatial
{
	
	private Sprite cursor;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScriptEditor scriptEditor = (ScriptEditor)FindNode("ScriptEditor");				
		Robot robot = (Robot)FindNode("Robot");

		

		// Resource plus = ResourceLoader.Load("res://plus.png");
		// Input.SetCustomMouseCursor(plus);

		cursor = (Sprite)FindNode("Cursor");
		HUD hud = (HUD)FindNode("HUD");

		// Hide os cursor
		Input.SetMouseMode(Input.MouseMode.Hidden);
		hud.SetCursor(Cursor.Pointer);
				
		scriptEditor._Init(robot);
	}

    public override void _Process(float delta)
    {
		cursor.Position = GetViewport().GetMousePosition();

        base._Process(delta);
    }

}
