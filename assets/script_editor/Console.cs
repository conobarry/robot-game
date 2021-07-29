using Godot;

public class Console : RichTextLabel
{
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }
    
    public void Print(string text)
    {
        // this.AddText(text);
        this.AppendBbcode($"[code]{text}[/code]");
    }

    public void PrintLine(string text)
    {
        this.Print(text);
        this.Print("\n");
    }
    
    public void PrintError(string text)
    {
        this.Print($"\n[color=red]{text}[/color]");
    }

}
