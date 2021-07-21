using Godot;

public class TestLevel : Spatial
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScriptEditor scriptEditor = GetNode<ScriptEditor>(new NodePath(typeof(ScriptEditor).Name));
				
		Robot robot = GetNode<Robot>(new NodePath(typeof(Robot).Name));
				
		scriptEditor._Init(robot);
	}

}
