using Godot;

public class CodeEditor : TextEdit
{
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _Process(float delta)
    {
        // GD.Print(GetTree().Root.FindNode("HUD").Name);

        base._Process(delta);
    }

}
