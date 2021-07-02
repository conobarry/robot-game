using Godot;

public class ScriptEditor : CanvasLayer
{
    
    public Robot robot;
    private RunButton runButton;
    private Console console;
    private PythonScriptManager scriptManager;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {        
        this.runButton = GetNode<RunButton>(new NodePath("Window/VBoxContainer/InputContainer/RunButton"));        
        this.console = GetNode<Console>(new NodePath("Window/VBoxContainer/InputContainer/EditorContainer/Console"));
        this.scriptManager = new PythonScriptManager();
    }
    
    public ScriptEditor _Init(Robot robot)
    {                
        this.robot = robot;
        this.runButton.Connect(nameof(RunButton.OnRunScript), scriptManager, nameof(PythonScriptManager.RunScript), new Godot.Collections.Array {robot, "test", console});
                
        return this;
    }

}
