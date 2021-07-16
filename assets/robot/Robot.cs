using System.Collections.Generic;
using Godot;

public class Robot : KinematicBody
{

    [Export]
    private Vector3 gravity = Vector3.Down * 10;
    [Export]
    private int speed = 4;
    [Export]
    private double rotationalSpeed = 0.85;
    [Export]
    private Vector3 velocity = Vector3.Zero;

    private Movement movement = null;

    private float counter = 0f;

    public bool isBusy = false;


    private class Movement
    {

        public enum Direction
        {
            None,
            Forward,
            Backward
        }

        public Direction direction = Direction.Forward;

        public int distance = 0;

        public Transform newTransform = Transform.Identity;
        public Movement() : this(Direction.None, 0, Transform.Identity)
        {
        }

        public Movement(Direction direction, int distance, Transform newTransform)
        {
            this.direction = direction;
            this.distance = distance;
            this.newTransform = newTransform;
        }
        
    }


    public override void _Ready()
    {
        // forward = Transform.basis.z;
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector3 direction = Vector3.Zero;

        if (this.movement != null)
        {

            if (this.Transform != this.movement.newTransform)
            {
                float t = 0f;
                t += delta * 4f;
                
                this.Transform = this.Transform.InterpolateWith(this.movement.newTransform, t);
            }
            else
            {
                this.movement = null;
                this.isBusy = false;
            }

            // if (this.movement.direction != Movement.Direction.None)
            // {
            //     if (this.movement.direction == Movement.Direction.Forward)
            //     {
            //         direction = -Transform.basis.z;
            //     }
            // }
        
        }        

        // this.velocity += direction * speed * delta;

        // Apply gravity
        this.velocity += gravity * delta;

        MoveAndSlide(velocity, Vector3.Up);
    }

    public void MoveForward(int distance)
    {
        GD.Print("Moving forward ", distance);        

        Vector3 forward = -Transform.basis.z;

        // Transform t = Transform;
        // t.origin += t.basis.x * 5;
        // Transform = t;

        Transform newTransform = this.Transform.Translated(forward * distance);

        if (this.movement == null)
        {
            this.movement = new Movement(Movement.Direction.Forward, distance, newTransform);
        }
    }

    public void MoveBackward(int distance)
    {
        GD.Print("Moving backward ", distance);

        Vector3 backward = Transform.basis.z;

        Transform newTransform = this.Transform.Translated(backward * distance);

        if (this.movement == null)
        {
            this.movement = new Movement(Movement.Direction.Backward, distance, newTransform);
        }
        // isForward = false;

    }

    public void Command(string command)
    {
        // Stack<string> commandArray = new Stack<string>(command.Split(' '));
        // string operation = commandArray.Pop();

        List<string> commandList = new List<string>(command.Split(' '));
        string operation = commandList[0];
        commandList.RemoveAt(0);

        switch (operation)
        {
            case "move-forward":
                MoveForward(commandList[0].ToInt());
                break;
        }
    }

    // public void Rotate(int degrees)
    // {
    //     this.Ro
    // }
}