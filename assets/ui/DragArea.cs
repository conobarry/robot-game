using Godot;
using System;

public class DragArea : Panel
{    

    public event EventHandler<InputEventMouseMotion> Drag;

    private bool isDragActive = false;

    private Vector2 prevMouseLoc = Vector2.Zero;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    public override void _GuiInput(InputEvent @event)    
    {
        if (@event is InputEventMouseButton)
        {
            InputEventMouseButton mouseEvent = (InputEventMouseButton)@event;

            if (mouseEvent.ButtonIndex == (int)ButtonList.Left)
            {

                if (mouseEvent.Pressed)
                {
                    isDragActive = true;
                }
                else
                {
                    isDragActive = false;
                }
            }
        }
        else if (@event is InputEventMouseMotion)
        {
            InputEventMouseMotion mouseMotion = (InputEventMouseMotion)@event;            

            // GD.Print("move");
            if (isDragActive)
            {
                OnDrag(mouseMotion);
            }
            
        }

        base._GuiInput(@event);
    }

    protected virtual void OnDrag(InputEventMouseMotion args)
    {
        EventHandler<InputEventMouseMotion> handler = Drag;
        if (handler != null)
        {
            handler(this, args);
        }
    }

}