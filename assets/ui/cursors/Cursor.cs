using Godot;

public class Cursor : Sprite
{
    // public Vector2 Offset { get; set; }

    // public Texture Texture { get; set; }

    public static Cursor Arrow;

    public static Cursor Pointer;

    public static Cursor IBeam;
    
    public static Cursor Cross;

    public static Cursor Ruler;

    private Cursor(Texture texture, Vector2 offset)
    {
        this.Texture = texture;
        this.Offset = offset;
    }

    public override void _Ready()
    {
        Texture arrowTexture = GD.Load<Texture>("res://assets/ui/cursors/arrow.png");
        Arrow = new Cursor(arrowTexture, new Vector2(12, 12));

        Texture pointerTexture = GD.Load<Texture>("res://assets/ui/cursors/pointer.png");
        Pointer = new Cursor(pointerTexture, new Vector2(6, 12));

        Texture rulerTexture = GD.Load<Texture>("res://assets/ui/cursors/ruler.png");
        Ruler = new Cursor(rulerTexture, new Vector2(7, 11));
    }
}

// public enum CursorType
// {
//     Default,
//     Arrow,
//     Pointer,
//     IBeam,
//     Cross,
//     Ruler        
// }