// Go here to see what I'm doing: https://stackoverflow.com/questions/3055002/how-can-i-redirect-the-stdout-of-ironpython-in-c

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
    
    public string ReadEnvironment()
    {
        string filePath = "res://assets/script_editor/python/env.py";
        // Resource pythonFile = ResourceLoader.Load(filePath, nameof(TextFile));
        
        Godot.File pythonFile = new Godot.File();
        pythonFile.Open(filePath, Godot.File.ModeFlags.Read);
        string script = pythonFile.GetAsText();
        pythonFile.Close();
        return script;
        
    }
    
    private void StringWritten(object sender, StringWrittenEventArgs<string> e)
    {
        GD.Print(e.Value);
    }
    
    public void RunScript(Robot robot, string script, Console console)
    {
        GD.Print("Executing script");
        
        // Create an IronPython script engine
        ScriptEngine scriptEngine = IronPython.Hosting.Python.CreateEngine();
        
        // Add the python files to the search path
        var paths = scriptEngine.GetSearchPaths();
        paths.Add("assets/script_editor/python/");
        scriptEngine.SetSearchPaths(paths);        
        
        // Set the python variable cs_robot to be a c# robot object
        scriptEngine.Runtime.Globals.SetVariable("cs_robot", robot);
        
        ScriptScope scriptScope = scriptEngine.CreateScope();
                                 
        // Execute a python file and store it in the scope
        scriptEngine.ExecuteFile("assets/script_editor/python/robot.py", scriptScope);
        scriptEngine.ExecuteFile("assets/script_editor/python/test_script.py", scriptScope);
        
        // These lines make it so the output of the script can be captured        
        MemoryStream streamOut = new MemoryStream();
        MemoryStream streamErr = new MemoryStream();
        
        AutomaticStreamWriter writer = new AutomaticStreamWriter(streamOut);
        writer.StringWritten += new EventHandler<StringWrittenEventArgs<string>>(StringWritten);
        
        // StreamWriter writer = new StreamWriter(System.Console.OpenStandardOutput());
        // writer.AutoFlush = true;
        
        scriptEngine.Runtime.IO.SetOutput(streamOut, writer);
        
        // scriptEngine.Runtime.IO.SetOutput(streamOut, Encoding.Default);
        scriptEngine.Runtime.IO.SetErrorOutput(streamErr, Encoding.Default);
        // scriptEngine.Operations.Invoke(testFn);
        
        string testScript = @"print('Running test script')

def test():
    print('This is a test.')

test()
";

        dynamic testFn = scriptScope.GetVariable("test");
        
        // StreamReader reader = new StreamReader(streamOut, Encoding.Default);
        
        // streamOut.BeginRead()
        
        try
        {                                       
            // Execute a python script from a string and store it in the scope
            // dynamic returned = scriptEngine.Execute(testScript, scriptScope);
            
            dynamic returned = scriptEngine.Operations.Invoke(testFn);
            
            dynamic stdout = Encoding.Default.GetString(streamOut.ToArray());
            dynamic stderr = Encoding.Default.GetString(streamErr.ToArray());
            
            // GD.Print("returned       : ", returned);
            // GD.Print("captured stdout: ", stdout);
            // GD.Print("captured stderr: ", stderr);
        }
        catch (Exception e)
        {
            ExceptionOperations eo = scriptEngine.GetService<ExceptionOperations>();
            string error = eo.FormatException(e);
            console.PrintError(error);
            // GD.Print("Exception: ", error);
        }
        
        
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
        GD.Print("Running script");
        
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
        
        // process.ErrorDataReceived += Process_ErrorDataReceived;
        process.OutputDataReceived += Process_OutputDataReceived;
                
        process.Start();
        process.BeginOutputReadLine();
        // process.BeginErrorReadLine();
        
        // StreamReader streamReader = process.StandardOutput;        
        
        // StreamWriter streamWriter = process.StandardInput;
        // streamWriter.WriteLine("print(5)");
        
        OS.DelayMsec(100);
        
        process.StandardInput.WriteLine(ReadEnvironment());
                
        string testScript = @"print('Running test script')

def test():
    print('This is a test.')

test()
die()";
        
        process.StandardInput.WriteLine(testScript);
        
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
        if (e.Data != null)
        {
            if (e.Data.StartsWith("#output"))
            {
                GD.Print("Output: ", e.Data);
            }
            else if (e.Data.StartsWith("#error"))
            {
                GD.Print($"Error: {e.Data.Trim('>', ' ', '.')}");
            }
        }
        
    }
    
    static void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (e.Data != null)
        {
            if (e.Data.StartsWith(">>>"))
            {
                GD.Print($"Error: {e.Data.Trim('>', ' ', '.')}");
            }
            else
            {
                GD.Print(e.Data);
            }
        }
        
    }
    
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}