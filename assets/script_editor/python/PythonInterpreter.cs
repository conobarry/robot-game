using Godot;
using System.IO;
using File = Godot.File;

public class PythonInterpreter
{
    public Robot robot;
    
    public PythonInterpreter(Robot robot)
    {
        this.robot = robot;
    }
    
    public void ParseLine(string line)
    {
        GD.Print(line);
    }
    
    public void RunScript(string scriptPath)
    {        
        
        File file = new File();
        
        if (file.FileExists(scriptPath))
        {
            file.Open(scriptPath, File.ModeFlags.Read);
            string content = file.GetAsText();
            
            using (StringReader reader = new StringReader(content))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    ParseLine(line);
                }
            }
        } 
        else
        {
            // Raise exception
        }
        
    }
    
}