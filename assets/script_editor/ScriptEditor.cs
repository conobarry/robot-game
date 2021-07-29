using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Godot;

public class ScriptEditor : DebugUI
{

    public Robot robot;

    private RunButton runButton;

    private Console console;

    private PythonScriptManager scriptManager;

    private CodeEditor codeEditor;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // this.runButton = GetNode<RunButton>(new NodePath("Window/Panel/VBoxContainer/InputContainer/RunButton"));
        this.runButton = (RunButton)FindNode("RunButton");
        // this.textEditor = GetNode<TextEdit>(new NodePath("Window/Panel/VBoxContainer/InputContainer/EditorContainer/TextEditor"));
        this.codeEditor = (CodeEditor)FindNode("CodeEditor");
        // this.console = GetNode<Console>(new NodePath("Window/Panel/VBoxContainer/InputContainer/EditorContainer/Console"));
        this.console = (Console)FindNode("Console");
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
        runButton.Disabled = true;

        scriptManager._InitSharp(robot, console);
        // scriptManager._InitIron(robot, console);

        codeEditor.Readonly = true;

        string script = codeEditor.Text;
        int numLines = codeEditor.GetLineCount();

        for (int lineNum = 0; lineNum < numLines; lineNum++)
        {
            while (robot.isBusy) {}

            string line = codeEditor.GetLine(lineNum);
            codeEditor.Select(lineNum, 0, lineNum, line.Length);
            // var execute = scriptManager.Execute(line);
            // robot.isBusy = true;
            await Task.Run(() => scriptManager.Execute(line));
            OS.DelayMsec(1000);
            GD.Print("Line executed");
        }

        codeEditor.Deselect();
        codeEditor.Readonly = false;

        runButton.Disabled = false;

    }

    // Processes sysout from python and sends anything useful as an event
    public void ProcessSysOut(object sender, DataReceivedEventArgs args)
    {
        string text = args.Data;

        if (text != null && text != "")
        {
            GD.Print("Output received: ", text);

            if (text.StartsWith("@print:"))
            {
                console.PrintLine(text.Replace("@print:", ""));
            }
            else if (text.StartsWith("@command:"))
            {
                text = text.Replace("@command:", "");
                List<string> commands = new List<string>(text.Split(' '));

                string operation = commands[0];
                commands.RemoveAt(0);

                if (commands.Count > 0)
                {
                    robot.Command(operation, commands.ToArray());
                }
                else
                {
                    robot.Command(operation);
                }
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
