using Godot;
using System;
using System.IO;
using System.Text;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Diagnostics;

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
    
    public void RunScriptExternal(Robot robot, string script)
    {
        Godot.Collections.Array output = new Godot.Collections.Array();
        int exitCode = OS.Execute("python3", new string[] {"--version"}, false, output);
        GD.Print("exit code: " + exitCode);
        GD.Print(output);        
               
        
    }
    
    public void RunScriptSharp(Robot robot, string script)
    {
        Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/usr/bin/python3",
                Arguments = "-i",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                CreateNoWindow = true
            },
            EnableRaisingEvents = true
        };
        
        process.ErrorDataReceived += Process_OutputDataReceived;
        process.OutputDataReceived += Process_OutputDataReceived;
        
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        
        // StreamReader streamReader = process.StandardOutput;        
        
        // StreamWriter streamWriter = process.StandardInput;
        // streamWriter.WriteLine("print(5)");
        process.StandardInput.WriteLine("def test():");
        process.StandardInput.WriteLine("   print(20)");
        process.StandardInput.WriteLine("");
        process.StandardInput.WriteLine("test()");
        process.StandardInput.WriteLine("exit()");
        
        // string output = process.StandardOutput.ReadToEnd();
        // GD.Print(output);
        
        // streamWriter.Close();
        // streamReader.Close();
        // GD.Print(output); 
        // process.Kill();        
        
        // process.Close();
        // process.WaitForExit();
        
        // Console.Read();               
        
        // string command = "-u";
        // using (Process process = new Process())
        // {
        //     process.StartInfo.FileName = "/usr/bin/python3";
        //     process.StartInfo.UseShellExecute = false;
        //     process.StartInfo.RedirectStandardInput = true;
        // }
        
        // while (true)
        // {
            
        // }
    }
    
    static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (e.Data != null) {GD.Print(e.Data);}
        // Console.WriteLine(e.Data);
    }
    
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}