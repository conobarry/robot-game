using Godot;

public class RunButton : Button
{        
    
    [Signal]
    public delegate void OnRunScript();
    
    public void OnButtonPressed()
    {
        EmitSignal(nameof(OnRunScript));
    }
}