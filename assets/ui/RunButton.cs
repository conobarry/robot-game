using System;
using Godot;

public class RunButton : Button
{        
    
    // [Signal]
    // public delegate void OnRunScript();

    public event EventHandler ButtonPressed;
    
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