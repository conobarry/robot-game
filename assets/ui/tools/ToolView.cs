using Godot;

public class ToolView : ViewportContainer
{
    private MeasureLine measureLine;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        measureLine = (MeasureLine) FindNode("MeasureLine");
    }

    public void _Init(Cursor cursor)
    {
        measureLine._Init(cursor);
    }

}
