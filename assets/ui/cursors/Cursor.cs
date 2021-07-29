using Godot;

public class Cursor : Control
{
    private struct CursorData
    {
        public Texture Texture { get; set; }

        public Vector2 Center { get; set; }

        public CursorData(Texture texture, Vector2 center = default(Vector2))
        {
            this.Texture = texture;
            this.Center = center;
        }
    }

    public Label TooltipUpper { get; private set; }

    public Label TooltipLower { get; private set; }

    public Texture Texture
    {
        get { return sprite.Texture; }
        set { sprite.Texture = value; }
    }

    public Vector2 Center
    { 
        get
        { 
            return -sprite.Offset;             
        }
        set
        {
            sprite.Offset = -value;
            Vector2 newPosition = new Vector2(centerContainer.RectPosition.x, centerContainer.RectPosition.y);
            centerContainer.SetPosition(newPosition);
        }
    }

    private CenterContainer centerContainer;

    private Sprite sprite;

    private CursorData defaultCursor;

    private CursorData arrow;

    private CursorData pointer;

    private CursorData grab;

    private CursorData hand;

    private CursorData iBeam;
    
    private CursorData cross;

    private CursorData ruler;


    public override void _Ready()
    {
        sprite = (Sprite) FindNode("Sprite");

        centerContainer = (CenterContainer) FindNode("CenterContainer");

        TooltipUpper = (Label) FindNode("UpperTooltip");
        TooltipLower = (Label) FindNode("LowerTooltip");

        // Hide os cursor
		Input.SetMouseMode(Input.MouseMode.Hidden);

        Texture arrowTexture = GD.Load<Texture>("res://assets/ui/cursors/arrow.png");
        arrow = new CursorData(arrowTexture, new Vector2(0, 0));

        Texture pointerTexture = GD.Load<Texture>("res://assets/ui/cursors/pointer.png");
        pointer = new CursorData(pointerTexture, new Vector2(6, 0));

        Texture rulerTexture = GD.Load<Texture>("res://assets/ui/cursors/ruler.png");
        ruler = new CursorData(rulerTexture, new Vector2(5, 0));

        Texture beamTexture = GD.Load<Texture>("res://assets/ui/cursors/beam.png");
        iBeam = new CursorData(beamTexture, new Vector2(11, 11));

        Texture crossTexture = GD.Load<Texture>("res://assets/ui/cursors/cross.png");
        cross = new CursorData(crossTexture, new Vector2(16, 16)); 

        Texture grabTexture = GD.Load<Texture>("res://assets/ui/cursors/grab.png");
        grab = new CursorData(grabTexture, new Vector2(6, 0));

        Texture handTexture = GD.Load<Texture>("res://assets/ui/cursors/hand.png");
        hand = new CursorData(handTexture, new Vector2(6, 0)); 

        defaultCursor = arrow;
    }

    public override void _Process(float delta)
    {
        RectPosition = GetViewport().GetMousePosition();

        base._Process(delta);
    }

    public void SetCursor(CursorType cursorType)
    {
        CursorData newCursor;
        
        switch (cursorType)
        {
            
            case CursorType.Arrow:
                newCursor = arrow;
                break;
            case CursorType.Pointer:
                newCursor = pointer;
                break;
            case CursorType.Grab:
                newCursor = grab;
                break;
            case CursorType.Hand:
                newCursor = hand;
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
            case CursorType.Default:
            default:
                newCursor = defaultCursor;
                break;
        }

        this.Texture = newCursor.Texture;
        this.Center = newCursor.Center;
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
    Grab,
    Hand,
    IBeam,
    Cross,
    Ruler
}