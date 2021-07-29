using Godot;

public class ToolCamera : Camera
{
    public override void _Process(float delta)
    {
        // Make this camera follow the main level camera
        Transform = this.GetTree().Root.GetCamera().Transform;

        base._Process(delta);
    }
}
