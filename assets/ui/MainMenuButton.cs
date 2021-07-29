using Godot;
using System;

public class MainMenuButton : ToolButton
{

    public event EventHandler ButtonPressed;
    public event EventHandler ButtonDown;
    public event EventHandler ButtonUp;

    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Connect("pressed", this, nameof(OnButtonPressed));
        Connect("button_down", this, nameof(OnButtonDown));
        Connect("button_up", this, nameof(OnButtonUp));
    }

    protected virtual void OnButtonPressed()
    {
        EventHandler handler = ButtonPressed;
        if (handler != null)
        {
            handler(this, new EventArgs());
        }
    }
    protected virtual void OnButtonDown()
    {
        EventHandler handler = ButtonDown;
        if (handler != null)
        {
            handler(this, new EventArgs());
        }
    }

    protected virtual void OnButtonUp()
    {
        EventHandler handler = ButtonUp;
        if (handler != null)
        {
            handler(this, new EventArgs());
        }
    }
}
