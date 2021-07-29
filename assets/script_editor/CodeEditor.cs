using Godot;

public class CodeEditor : TextEdit
{
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Connect("mouse_entered", this, nameof(OnMouseEnter));
        Connect("mouse_exited", this, nameof(OnMouseExit));
    }

    private void OnMouseEnter()
    {
        GetTree().GetCursor().SetCursor(CursorType.IBeam);
    }

    private void OnMouseExit()
    {
        GetTree().GetCursor().SetCursor(CursorType.Default);
    }

}
