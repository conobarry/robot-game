using Godot;

public class InputEventMouseResize : InputEventMouseMotion
{
    
    public OrdinalDirection Direction { get; set; }

    public InputEventMouseResize() {}

    public InputEventMouseResize(InputEventMouseMotion motionEvent, OrdinalDirection direction = OrdinalDirection.None)
    {
        this.Device = motionEvent.Device;
        this.Tilt = motionEvent.Tilt;
        this.Pressure = motionEvent.Pressure;
        this.Relative = motionEvent.Relative;
        this.Speed = motionEvent.Speed;
        this.ButtonMask = motionEvent.ButtonMask;
        this.Position = motionEvent.Position;
        this.GlobalPosition = motionEvent.GlobalPosition;
        this.Alt = motionEvent.Alt;
        this.Shift = motionEvent.Shift;
        this.Control = motionEvent.Control;
        this.Command = motionEvent.Command;
    }

    public InputEventMouseResize(OrdinalDirection direction)
    {
        this.Direction = direction;
    }
}
