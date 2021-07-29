using Godot;

public class TestLevel : Spatial
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()	
	{
		Robot robot = (Robot) FindNode("Robot");

		ScriptEditor scriptEditor = (ScriptEditor) FindNode("ScriptEditor");
		scriptEditor._Init(robot);		

		// Set new cursor
		Cursor cursor = (Cursor) FindNode("Cursor");
		cursor.SetCursor(CursorType.Arrow);
		
		MainCamera camera = (MainCamera) FindNode("MainCamera");
		camera._Init(cursor);

		ToolView toolView = (ToolView) FindNode("ToolView");
		toolView._Init(cursor);
	}

}
