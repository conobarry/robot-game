using System;
using Godot;

public class RunButton : Button
{    

    public event EventHandler ButtonPressed;

    public override void _Ready()
    {
        Connect("mouse_entered", this, nameof(OnMouseEnter));
        Connect("mouse_exited", this, nameof(OnMouseExit));
    }

    private void OnMouseEnter()
    {
        GetTree().GetCursor().SetCursor(CursorType.Pointer);
    }

    private void OnMouseExit()
    {
        GetTree().GetCursor().SetCursor(CursorType.Default);
    }

    public virtual void OnButtonPressed()
    {
        // EmitSignal(nameof(OnRunScript));
        EventHandler handler = ButtonPressed;
        // handler?.Invoke(this, e);
        if (handler != null)
        {
            handler(this, EventArgs.Empty);
        }
    }
}