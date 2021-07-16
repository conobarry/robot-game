using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Godot;

public class ScriptEditor : CanvasLayer
{

    public Robot robot;

    private RunButton runButton;

    private Console console;

    private PythonScriptManager scriptManager;

    private TextEdit textEditor;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.runButton = GetNode<RunButton>(new NodePath("Window/VBoxContainer/InputContainer/RunButton"));
        this.textEditor = GetNode<TextEdit>(new NodePath("Window/VBoxContainer/InputContainer/EditorContainer/TextEditor"));
        this.console = GetNode<Console>(new NodePath("Window/VBoxContainer/InputContainer/EditorContainer/Console"));
        this.scriptManager = new PythonScriptManager();

    }

    public ScriptEditor _Init(Robot robot)
    {
        this.robot = robot;
        // this.runButton.Connect(nameof(RunButton.OnRunScript), this, nameof(ScriptEditor.RunScriptAsync), new Godot.Collections.Array { robot });

        runButton.ButtonPressed += async (sender, e) => await Task.Run(() => RunScriptAsync(robot));
        scriptManager.SysOutReceived += ProcessSysOut;
        scriptManager.SysErrReceived += ProcessSysErr;

        return this;
    }

    public async void RunScriptAsync(Robot robot)
    {
        scriptManager._InitSharp(robot, console);
        // scriptManager._InitIron(robot, console);

        textEditor.Readonly = true;

        string script = textEditor.Text;
        int numLines = textEditor.GetLineCount();

        for (int lineNum = 0; lineNum < numLines; lineNum++)
        {
            string line = textEditor.GetLine(lineNum);
            textEditor.Select(lineNum, 0, lineNum, line.Length);
            // var execute = scriptManager.Execute(line);
            await Task.Run(() => scriptManager.Execute(line));
            GD.Print("Line executed");
        }

        textEditor.Deselect();
        textEditor.Readonly = false;

    }

    // Processes sysout from python and sends anything useful as an event
    public void ProcessSysOut(object sender, DataReceivedEventArgs args)
    {
        string text = args.Data;

        if (text != null)
        {
            GD.Print("Output received: ", text);

            switch (text)
            {
                case string s when s.StartsWith("@print"):
                    console.PrintLine(text.Replace("@print", ""));
                    break;
                case string s when s.StartsWith("@command"):
                    robot.Command(text.Replace("@command", ""));
                    break;
            }
        }

    }

    public void ProcessSysErr(object sender, DataReceivedEventArgs args)
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



}
