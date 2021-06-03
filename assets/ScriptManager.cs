using Godot;
using System;
public abstract class ScriptManager : Node
{    
    private Robot robot;
        
    public override void _Ready()
    {
        Connect("run_script", this, "RunScript");
    }
    
    public abstract void RunScript(string script);

}