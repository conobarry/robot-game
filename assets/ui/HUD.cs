using Godot;

public class HUD : CanvasLayer
{
    private Vector2 measureStart;

    private Sprite cursor;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        cursor = (Sprite)FindNode("Cursor");
    }

    public void SetCursor(Cursor cursor)
    {
        this.cursor = cursor;
    }


}
