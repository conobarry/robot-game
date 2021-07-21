using Godot;

public class DebugUI : Godot.CanvasLayer
{
    private Robot robot;
    private Label debugLabel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.robot = GetParent().GetNode<Robot>(new NodePath("Robot"));
        this.debugLabel = GetNode<Label>(new NodePath("DebugLabel"));
    }

    public override void _Process(float delta)
    {
        if (robot != null)
        {
            Vector3 position = robot.Transform.origin;
            string x = position.x.ToString("F2");
            string y = position.y.ToString("F2");
            string z = position.z.ToString("F2");
            debugLabel.Text = $"({x}, {y}, {z})";
        }
        
        base._Process(delta);
    }

}
