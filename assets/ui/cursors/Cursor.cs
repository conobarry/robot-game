using Godot;

public class Cursor : Sprite
{
    public Label TooltipUpper { get; private set; }

    public Label TooltipLower { get; private set; }

    private Cursor defaultCursor;

    private Cursor arrow;

    private Cursor pointer;

    private Cursor iBeam;
    
    private Cursor cross;

    private Cursor ruler;

    public Cursor()
    {

    }

    private Cursor(Texture texture, Vector2 offset)
    {
        this.Texture = texture;
        this.Offset = offset;
    }

    public override void _Ready()
    {
        TooltipUpper = (Label) FindNode("UpperTooltip");
        TooltipLower = (Label) FindNode("LowerTooltip");

        // Hide os cursor
		Input.SetMouseMode(Input.MouseMode.Hidden);

        Texture arrowTexture = GD.Load<Texture>("res://assets/ui/cursors/arrow.png");
        arrow = new Cursor(arrowTexture, new Vector2(12, 12));

        Texture pointerTexture = GD.Load<Texture>("res://assets/ui/cursors/pointer.png");
        pointer = new Cursor(pointerTexture, new Vector2(6, 12));

        Texture rulerTexture = GD.Load<Texture>("res://assets/ui/cursors/ruler.png");
        ruler = new Cursor(rulerTexture, new Vector2(7, 11));

        Texture beamTexture = GD.Load<Texture>("res://assets/ui/cursors/beam.png");
        iBeam = new Cursor(beamTexture, new Vector2(1, 1));

        Texture crossTexture = GD.Load<Texture>("res://assets/ui/cursors/cross.png");
        cross = new Cursor(crossTexture, new Vector2(0, 0));        

        defaultCursor = arrow;
    }

    public override void _Process(float delta)
    {
        Position = GetViewport().GetMousePosition();

        base._Process(delta);
    }

    public void SetCursor(CursorType cursorType)
    {
        Cursor newCursor = defaultCursor;
        
        switch (cursorType)
        {
            case CursorType.Default:
                newCursor = defaultCursor;
                break;
            case CursorType.Arrow:
                newCursor = arrow;
                break;
            case CursorType.Pointer:
                newCursor = pointer;
                break;
            case CursorType.IBeam:
                newCursor = iBeam;
                break;
            case CursorType.Cross:
                newCursor = cross;
                break;
            case CursorType.Ruler:
                newCursor = ruler;
                break;
        }

        this.Texture = newCursor.Texture;
        this.Offset = newCursor.Offset;
    }

    public void SetDefault()
    {
        SetCursor(CursorType.Default);
    }
}

public enum CursorType
{
    Default,
    Arrow,
    Pointer,
    IBeam,
    Cross,
    Ruler        
}