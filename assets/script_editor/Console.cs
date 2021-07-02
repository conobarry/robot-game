using Godot;

public class Console : RichTextLabel
{
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }
    
    public void Print(string text)
    {
        this.AddText(text);
    }
    
    public void PrintError(string text)
    {
        this.AppendBbcode($"\n[color=red]{text}[/color]");
    }

}
