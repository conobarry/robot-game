using System.Collections.Generic;
using Godot;

public class Robot : KinematicBody
{

    [Export]
    private Vector3 gravity = Vector3.Down * 4;

    [Export]
    private int speed = 4;

    [Export]
    private double rotationalSpeed = 0.85;

    [Export]
    private Vector3 velocity = Vector3.Zero;

    // private MoveCommand moveCommand = null;

    private Queue<MoveCommand> commands = new Queue<MoveCommand>();

    public bool isBusy = false;

    public Vector3 Forward { get { return -Transform.basis.z; } }

    public Vector3 Backward { get { return Transform.basis.z; } }

    public Vector3 Left { get { return Transform.basis.x; } }

    public Vector3 Right { get { return -Transform.basis.x; } }

    public Vector2 GroundPosition
    {
        get { return new Vector2(Transform.origin.x, Transform.origin.z); }
    }


    private class MoveCommand
    {

        public bool finished = false;

        public Vector3 direction;

        public float distance = 0;

        public readonly Vector2 targetGroundPosition;

        public Vector3 target;

        public readonly Transform newTransform;

        public float movedDistance;

        // public Vector3 translation;

        // public MoveCommand() : this(Direction.None, Vector3.Zero) {}

        // public MoveCommand(Direction direction, Vector3 target)
        // {
        //     this.direction = direction;
        //     this.newGroundPosition = target;
        // }

        // public MoveCommand(Direction direction, Transform newTransform)
        // {
        //     this.direction = direction;
        //     this.newTransform = newTransform;
        // }

        public MoveCommand(Robot robot, Vector3 direction, float distance)
        {
            this.direction = direction;
            this.distance = distance;

            // Vector3 vectorDirection;

            // switch (direction)
            // {
            //     case Direction.Forward:
            //         vectorDirection = robot.Forward;
            //         break;
            //     case Direction.Backward:
            //         vectorDirection = robot.Backward;
            //         break;
            //     default:
            //         // Error
            //         return;
            // }

            // vectorDirection = vectorDirection.Normalized();

            // Transform t = Transform;
            // t.origin += t.basis.x * 5;
            // Transform = t;

            newTransform = robot.Transform.Translated(direction * distance);

            // GD.Print("current pos: ", Transform.origin);

            targetGroundPosition = GetGroundPosition(newTransform.origin);

            // translation = robot.Translation;
        }
        
    }

    public static Vector2 GetGroundPosition(Vector3 position)
    {
        return new Vector2(position.x, position.z);
    }


    public override void _Ready()
    {
        // forward = Transform.basis.z;
    }

    // private float movedDistance = 0f;

    public override void _PhysicsProcess(float delta)
    {
        Vector3 thisMovement = Vector3.Zero;

        bool finishedMovement = false;

        // Apply gravity
        // thisMovement += (gravity * delta);
        // thisMovement += gravity;

        if (commands.Count > 0)
        {
            MoveCommand moveCommand = commands.Peek();

            isBusy = !moveCommand.finished;

            if (moveCommand.movedDistance < moveCommand.distance)
            {

                float t = delta * 1f;                
                thisMovement += (moveCommand.direction * t);
                // thisMovement += moveCommand.direction;

                // movedDistance += Mathf.Abs(new Vector3(thisMovement.x, 0, thisMovement.z).Length());
                moveCommand.movedDistance += Mathf.Abs(thisMovement.Length());

                // GD.Print($"{movedDistance}, {thisMovement}");

                // If the distance to move this frame would move the robot too far
                if (moveCommand.movedDistance > moveCommand.distance)
                {
                    // Reduce the movement vector so this doesn't happen
                    float diff = moveCommand.movedDistance - moveCommand.distance;
                    thisMovement *= (1 - diff / thisMovement.Length());

                    finishedMovement = true;                    
                }
                else if (moveCommand.movedDistance == moveCommand.distance)
                {
                    finishedMovement = true;
                }               
                
                // Vector3 newPosition = GetGroundPosition(Transform.origin + movement);

                // Vector3 newVector = moveCommand.translation.LinearInterpolate(new Vector3(0, 0, -3), t);
                // TranslateObjectLocal(Forward * moveCommand.distance);
                
                // Transform = Transform.InterpolateWith(moveCommand.newTransform, t);
            }
            else
            {
                finishedMovement = true;
            }

            // if (this.movement.direction != Movement.Direction.None)
            // {
            //     if (this.movement.direction == Movement.Direction.Forward)
            //     {
            //         direction = -Transform.basis.z;
            //     }
            // }

            if (finishedMovement)
            {
                moveCommand.finished = true;
                commands.Dequeue();
                isBusy = false;
                finishedMovement = false;
            }
        
        }

        // GD.Print(thisMovement);

        KinematicCollision collision = MoveAndCollide(thisMovement, false);
        // if (collision != null) {GD.Print(collision.Remainder);}        

        // MoveAndSlide(thisMovement);

        // this.velocity += direction * speed * delta;

        // Apply gravity
        // this.velocity += gravity * delta;

    }

    public void MoveForward(float distance)
    {
        GD.Print("Moving forward ", distance);
        commands.Enqueue(new MoveCommand(this, Forward, distance));
    }

    public void MoveBackward(float distance)
    {
        GD.Print("Moving backward ", distance);
        commands.Enqueue(new MoveCommand(this, Backward, distance));
    }

    public void Command(string operation, params string[] args)
    {
        if (args != null && args.Length > 0)
        {
            switch (operation)
            {
                case "move-forward":
                    MoveForward(args[0].ToInt());
                    break;
                case "move-backward":
                    MoveBackward(args[0].ToInt());
                    break;
            }
        }

        
    }

}