using Godot;
using System;
using System.Linq;

public class Window : MarginContainer
{

    private DragArea titlebar;

    private ResizeContainer resizeContainer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // this.titlebar = GetNode<Titlebar>(new NodePath("Panel/VBoxContainer/Titlebar"));
        this.titlebar = (DragArea) FindNode("DragArea");
        titlebar.Drag += Drag;

        this.resizeContainer = (ResizeContainer) FindNode("ResizeContainer");
        resizeContainer.Resize += Resize;
    }

    private void Drag(object sender, InputEventMouseMotion args)
    {
        Vector2 newPosition = RectPosition + args.Relative;

        Vector2 maxSize = GetViewport().GetVisibleRect().Size;

        if (newPosition.x < 0) { newPosition.x = 0; }
        if (newPosition.y < 0) { newPosition.y = 0; }
        if (newPosition.x + RectSize.x > maxSize.x) { newPosition.x = maxSize.x - RectSize.x; }
        if (newPosition.y + RectSize.y > maxSize.y) { newPosition.y = maxSize.y - RectSize.y; }

        RectPosition = newPosition;      
    }

    private void Resize(object sender, InputEventMouseResize args)
    {
        Vector2 size = RectSize;
        Vector2 position = RectPosition;
        Vector2 relative = args.Relative;
        CardinalDirection[] cardinals = args.Direction.GetCardinalComponents();

        foreach (CardinalDirection cardinal in cardinals)
        {
            switch (cardinal)
            {
                case CardinalDirection.North:
                    size.y -= relative.y;
                    position.y += relative.y;
                    break;
                case CardinalDirection.East:
                    size.x += relative.x;
                    break;
                case CardinalDirection.South:
                    size.y += relative.y;
                    break;
                case CardinalDirection.West:
                    size.x -= relative.x;
                    position.x += relative.x;
                    break;
            }
        }

        // Vector2 newPosition = RectPosition + args.Relative;

        // Vector2 maxSize = GetViewport().GetVisibleRect().Size;

        // GD.Print(RectSize.x);

        // if (newPosition.x < 0) { newPosition.x = 0; }
        // if (newPosition.y < 0) { newPosition.y = 0; }
        // if (newPosition.x + RectSize.x > maxSize.x) { newPosition.x = maxSize.x - RectSize.x; }
        // if (newPosition.y + RectSize.y > maxSize.y) { newPosition.y = maxSize.y - RectSize.y; }
        if (size.x > RectMinSize.x && size.y > RectMinSize.y)
        {
            RectPosition = position;
        }

        RectSize = size;
    }
    
}
