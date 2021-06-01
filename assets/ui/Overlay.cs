using Godot;
using System;
using System.IO;
using System.Text;
using IronPython.Hosting;
using Microsoft.Scripting;

public class Overlay : CanvasLayer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
        // GD.Print(this.GetClass());
        GD.Print("Hello from C# to Godot :)");

        // Create a script engine
        Microsoft.Scripting.Hosting.ScriptEngine scriptEngine = IronPython.Hosting.Python.CreateEngine();
        
        var paths = scriptEngine.GetSearchPaths();
        paths.Add("assets/python_library/");
        scriptEngine.SetSearchPaths(paths);        
        
        var scriptScope = scriptEngine.CreateScope();
        
        scriptScope.SetVariable("overlay", new Overlay());
        
        // Execute a python file and store it in the scope
        scriptEngine.ExecuteFile("assets/python_library/robot.py", scriptScope);
        scriptEngine.ExecuteFile("assets/ui/print.py", scriptScope);
        
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
        
        GD.Print("returned       : ", returned);
        GD.Print("captured stdout: ", stdout);
        GD.Print("captured stderr: ", stderr);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
