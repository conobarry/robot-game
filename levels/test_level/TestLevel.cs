using Godot;

public class TestLevel : Spatial
{
	PauseMenu pauseMenu;
	
	ProgressMeter progressMeter;

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
		camera._Init(cursor, robot);

		ToolView toolView = (ToolView) FindNode("ToolView");
		toolView._Init(cursor);

		pauseMenu = (PauseMenu) FindNode("PauseMenu");
		
		Godot.Collections.Array pads = GetTree().GetNodesInGroup("pad");
		foreach (Pad pad in pads)
		{
			pad.Connect("triggered", this, nameof(OnPadTriggered));
		}

		progressMeter = (ProgressMeter) FindNode("ProgressMeter");
		progressMeter._Init("Push the boxes!", 0, pads.Count);
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("pause"))
		{
			pauseMenu.Popup_();
			GetTree().Paused = true;
		}
	}

	private void OnPadTriggered()
	{
		progressMeter.CurrentValue += 1;
	}

}
