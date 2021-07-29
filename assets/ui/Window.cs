using Godot;
using System;

public class Window : MarginContainer
{

    private DragArea titlebar;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // this.titlebar = GetNode<Titlebar>(new NodePath("Panel/VBoxContainer/Titlebar"));
        this.titlebar = (DragArea)FindNode("DragArea");
        titlebar.Drag += Drag;
    }

    private void Drag(object sender, InputEventMouseMotion args)
    {
        Vector2 newPosition = RectPosition + args.Relative;

        Vector2 maxSize = GetViewport().GetVisibleRect().Size;

        GD.Print(RectSize.x);

        if (newPosition.x < 0) { newPosition.x = 0; }
        if (newPosition.y < 0) { newPosition.y = 0; }
        if (newPosition.x + RectSize.x > maxSize.x) { newPosition.x = maxSize.x - RectSize.x; }
        if (newPosition.y + RectSize.y > maxSize.y) { newPosition.y = maxSize.y - RectSize.y; }

        RectPosition = newPosition;      
    }

   
    
}
