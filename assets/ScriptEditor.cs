using Godot;

public class ScriptEditor : CanvasLayer
{
    
    public Robot robot;
    private RunButton runButton;
    private PythonScriptManager scriptManager;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.runButton = GetNode<RunButton>(new NodePath("Window/VBoxContainer/InputContainer/RunButton"));
        this.scriptManager = new PythonScriptManager();        
    }
    
    public ScriptEditor _Init(Robot robot)
    {        
        this.robot = robot;
        runButton.Connect(nameof(RunButton.OnRunScript), scriptManager, nameof(PythonScriptManager.RunScriptSharp), new Godot.Collections.Array {robot, "test"});
                
        return this;
    }

}
