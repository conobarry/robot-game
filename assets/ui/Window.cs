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
        RectPosition += args.Relative;
    }

   
    
}
