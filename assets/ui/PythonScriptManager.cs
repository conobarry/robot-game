using Godot;
using System;
using System.IO;
using System.Text;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

public class PythonScriptManager : Node
{   
    
    // [Signal]
    // public delegate void OnRunScript();
    
    // private Robot robot;
    
    // public PythonScriptManager(Robot robot)
    // {
    //     this.robot = robot;
    // }
    
    public override void _Ready()
    {
        // Connect("OnRunScript", this, "RunScript");
    }
    
    public void RunScript(Robot robot, string script)
    {
        GD.Print("Executing script");
        
        // Create a script engine
        ScriptEngine scriptEngine = IronPython.Hosting.Python.CreateEngine();
        
        var paths = scriptEngine.GetSearchPaths();
        paths.Add("assets/robot/python/");
        scriptEngine.SetSearchPaths(paths);        
        
        // Robot robot = new Robot();
        scriptEngine.Runtime.Globals.SetVariable("cs_robot", robot);
        
        var scriptScope = scriptEngine.CreateScope();
                         
        // Execute a python file and store it in the scope
        scriptEngine.ExecuteFile("assets/robot/python/robot.py", scriptScope);
        scriptEngine.ExecuteFile("assets/robot/python/test_script.py", scriptScope);
        
        // Execute a python script in a string and store it in the scope        
        // scriptEngine.Execute(script, scriptScope);
        
        var testFn = scriptScope.GetVariable("test");
        
        // These lines make it so the output of the script can be captured        
        var streamOut = new MemoryStream();
        var streamErr = new MemoryStream();        
        scriptEngine.Runtime.IO.SetOutput(streamOut, Encoding.Default);
        scriptEngine.Runtime.IO.SetErrorOutput(streamErr, Encoding.Default);
        
        // scriptEngine.Operations.Invoke(testFn);
        
        dynamic returned = scriptEngine.Operations.Invoke(testFn);
        dynamic stdout = Encoding.Default.GetString(streamOut.ToArray());
        dynamic stderr = Encoding.Default.GetString(streamErr.ToArray());
        
        // GD.Print("returned       : ", returned);
        // GD.Print("captured stdout: ", stdout);
        // GD.Print("captured stderr: ", stderr);
    }
    
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
