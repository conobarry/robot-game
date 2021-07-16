// Go here to see what I'm doing: https://stackoverflow.com/questions/3055002/how-can-i-redirect-the-stdout-of-ironpython-in-c

using Godot;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Diagnostics;

// public delegate void SysOutEventHandler(object sender, )

public class PythonScriptManager : Node
{   

    public event EventHandler<DataReceivedEventArgs> SysOutReceived;

    public event EventHandler<DataReceivedEventArgs> SysErrReceived;

    public Process process;
    public StreamWriter streamWriter;
    public StreamReader streamReader;
    
    public override void _Ready()
    {
        // Connect("OnRunScript", this, "RunScript");
    }
    
    public string ReadFile(string filePath)
    {
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
        
    public void Execute(string script)
    {
        GD.Print("Executing: ", script);
        process.StandardInput.WriteLine(script);
        OS.DelayMsec(2000);
        
        
        // streamWriter.WriteLine(script);
    }
    
    public void _InitSharp(Robot robot, Console console)
    {
        GD.Print("Setting up python environment");

        string currDirectory = System.Environment.CurrentDirectory;
        
        process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/usr/bin/python3",
                Arguments = "-iq",
                WorkingDirectory = $"{currDirectory}/assets/script_editor/python",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                CreateNoWindow = true
            },
            EnableRaisingEvents = true
        };
        
        // process.ErrorDataReceived += Process_ErrorDataReceived;
        // process.OutputDataReceived += Process_OutputDataReceived;
        // this.process.OutputDataReceived += (sender, args) => EmitSignal(nameof(OnSysOut));

        process.OutputDataReceived += (sender, args) => {
            OnSysOut(args);
        };

        process.ErrorDataReceived += (sender, args) => {
            OnSysErr(args);
        };
                
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        
        // streamReader = process.StandardOutput;        
        
        streamWriter = process.StandardInput;
        // streamWriter.WriteLine("print(5)");
        
        OS.DelayMsec(100);

        string envFile = "res://assets/script_editor/python/env.py";
        process.StandardInput.WriteLine(ReadFile(envFile));

        // string robotFile = "res://assets/script_editor/python/robot2.py";
        // this.process.StandardInput.WriteLine(ReadFile(robotFile));

        // string testFile = "res://assets/script_editor/python/test_script2.py";
        // process.StandardInput.WriteLine(ReadFile(testFile));

        // using (StringReader reader = new StringReader(ReadFile(testFile)))
        // {
        //     string line = "";
        //     while ((line = reader.ReadLine()) != null)
        //     {
        //         if (robot.isBusy == false)
        //         {
        //             process.StandardInput.WriteLine(line);
        //         }
                
        //     }
        // }
    }

    // Processes sysout from python and sends anything useful as an event
    public void Process_OutputDataReceived(object sender, DataReceivedEventArgs args)
    {
        GD.Print("Output received: ", args.Data);

        if (args.Data != null)
        {
            if (args.Data.StartsWith("@print"))
            {
                // OnSysOut(new EventArgs());
            }
            else if (args.Data.StartsWith("#error"))
            {
                GD.Print($"Error: {args.Data.Trim('>', ' ', '.')}");
            }
        }
        
    }
    
    public void Process_ErrorDataReceived(object sender, DataReceivedEventArgs args)
    {

        if (args.Data != null)
        {
            if (args.Data.StartsWith(">>>"))
            {
                GD.Print($"Error received: {args.Data.Trim('>', ' ', '.')}");
            }
            else
            {
                GD.Print(args.Data);
            }
        }
        
    }

    protected virtual void OnSysOut(DataReceivedEventArgs args)
    {
        EventHandler<DataReceivedEventArgs> handler = SysOutReceived;
        // handler?.Invoke(this, e);
        if (handler != null)
        {
            handler(this, args);
        }
    }

    protected virtual void OnSysErr(DataReceivedEventArgs args)
    {
        EventHandler<DataReceivedEventArgs> handler = SysErrReceived;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    public void _InitIron(Robot robot, Console console)
    {
        GD.Print("Setting up IronPython environment");
        
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
        // scriptEngine.ExecuteFile("assets/script_editor/python/test_script_iron.py", scriptScope);
        
        // These lines make it so the output of the script can be captured        
        MemoryStream streamOut = new MemoryStream();
        MemoryStream streamErr = new MemoryStream();
        
        AutomaticStreamWriter writer = new AutomaticStreamWriter(streamOut);
        writer.StringWritten += new EventHandler<StringWrittenEventArgs<string>>(StringWritten);

        // streamWriter = writer;
        
        // StreamWriter writer = new StreamWriter(System.Console.OpenStandardOutput());
        // writer.AutoFlush = true;
        
        scriptEngine.Runtime.IO.SetOutput(streamOut, writer);
        
        // scriptEngine.Runtime.IO.SetOutput(streamOut, Encoding.Default);
        scriptEngine.Runtime.IO.SetErrorOutput(streamErr, Encoding.Default);
        // scriptEngine.Operations.Invoke(testFn);
        
        // dynamic testFn = scriptScope.GetVariable("test");
        
        // StreamReader reader = new StreamReader(streamOut, Encoding.Default);
        
        // streamOut.BeginRead()
        
        try
        {                                       
            // Execute a python script from a string and store it in the scope
            // dynamic returned = scriptEngine.Execute(testScript, scriptScope);
            
            // dynamic returned = scriptEngine.Operations.Invoke(testFn);
            
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
    
    
    
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}