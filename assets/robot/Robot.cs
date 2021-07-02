using Godot;

public class Robot : KinematicBody {
    
    [Export]
    private Vector3 gravity = Vector3.Down * 10;
    [Export]
    private int speed = 4;
    [Export]
    private double rotationalSpeed = 0.85;
    [Export]    
    private Vector3 velocity = Vector3.Zero;
    
    
    private bool isForward = false;
    
    
    public override void _Ready()
    {
        // forward = Transform.basis.z;
    }
    
    public override void _PhysicsProcess(float delta)
    {
        velocity += gravity * delta;
        
        
        var vy = velocity.y;
        velocity = Vector3.Zero;
        
        if (isForward)
        {
            velocity += -Transform.basis.z * speed;
        }
        
        velocity.y = vy;
        
        
        velocity = MoveAndSlide(velocity, Vector3.Up);
    }
    
    public void MoveForward(int distance)
    {
        GD.Print("Moving forward ", distance);
        // this.Visible = false;
        
        // Rotate(Vector3.Up, 50);
        
        isForward = true;
        
        // for (int i = 0; i < 700; i++)
        // {
        //     GD.Print(i);
        // }
        
        // Vector3 direction = new Vector3(0, 1, 0);
        // MoveAndCollide(-this.Transform.basis.z * 5);
        // Translate(direction);
        // this.Translate(direction);
    }
    
    public void MoveBackward(int distance)
    {
        GD.Print("Moving backward ", distance);
        
        // isForward = false;
        
    }
    
    // public void Rotate(int degrees)
    // {
    //     this.Ro
    // }
}