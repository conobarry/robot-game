using System.Collections.Generic;
using Godot;

public class Robot : KinematicBody
{

    private abstract class RobotCommand
    {
        public bool finished = false;
    }

    private class RobotMoveCommand : RobotCommand
    {
        public Vector3 direction;

        public float distance;

        // public readonly Vector2 targetGroundPosition;

        // public Vector3 target;

        // public readonly Transform newTransform;

        public float movedDistance;

        public RobotMoveCommand(Robot robot, Vector3 direction, float distance)
        {
            this.direction = direction;
            this.distance = distance;

            // newTransform = robot.Transform.Translated(direction * distance);

            // targetGroundPosition = GetGroundPosition(newTransform.origin);
        }        
    }

    private class RobotTurnCommand : RobotCommand
    {
        public float angle;

        public Vector3 axis;

        public float progress;

        public RobotTurnCommand(Robot robot, Vector3 axis, float angle)
        {
            this.axis = axis;
            this.angle = angle;
        }
    }

    [Export]
    private Vector3 gravity = Vector3.Down * 4;

    [Export]
    private float speed = 1f;

    [Export]
    private float rotationalSpeed = 1f;


    private Queue<RobotCommand> commands = new Queue<RobotCommand>();

    private RobotCommand currentCommand;

    public bool isBusy = false;

    public Vector3 Forward { get { return Transform.basis.x; } }

    public Vector3 Backward { get { return -Transform.basis.x; } }

    public Vector3 Left { get { return -Transform.basis.z; } }

    public Vector3 Right { get { return Transform.basis.z; } }

    public Vector2 GroundPosition
    {
        get { return new Vector2(Transform.origin.x, Transform.origin.z); }
    }

    public static Vector2 GetGroundPosition(Vector3 position)
    {
        return new Vector2(position.x, position.z);
    }


    public override void _Ready() {}

    public override void _PhysicsProcess(float delta)
    {
        Vector3 thisMovement = Vector3.Zero;

        // bool finishedMovement = false;

        // Apply gravity
        // thisMovement += (gravity * delta);
        // thisMovement += gravity;

        if (commands.Count > 0)
        {
            RobotCommand command = commands.Peek();

            isBusy = !command.finished;

            switch (command)
            {
                case RobotMoveCommand moveCommand:
                    thisMovement = ProcessMoveCommand(moveCommand, delta, thisMovement);
                    break;
                case RobotTurnCommand turnCommand:
                    ProcessTurnCommand(turnCommand, delta);
                    break;
            }            

            if (command.finished)
            {
                commands.Dequeue();
                isBusy = false;
                // finishedMovement = false;
            }
        
        }

        KinematicCollision collision = MoveAndCollide(thisMovement);
        // if (collision != null) {GD.Print(collision.Remainder);}        

        // MoveAndSlide(thisMovement);

        // this.velocity += direction * speed * delta;

        // Apply gravity
        // this.velocity += gravity * delta;

    }

    public void MoveForward(float distance)
    {
        GD.Print($"Moving forward {distance}m");
        commands.Enqueue(new RobotMoveCommand(this, Forward, distance));
    }

    public void MoveBackward(float distance)
    {
        GD.Print($"Moving backward {distance}m");
        commands.Enqueue(new RobotMoveCommand(this, Backward, distance));
    }

    public void TurnRight(float angle)
    {
        GD.Print($"Turning right {angle}°");
        commands.Enqueue(new RobotTurnCommand(this, Vector3.Down, Mathf.Deg2Rad(angle)));        
    }

    public void TurnLeft(float angle)
    {
        GD.Print($"Turning left {angle}°");
        commands.Enqueue(new RobotTurnCommand(this, Vector3.Up, Mathf.Deg2Rad(angle)));  
    }

    public void Command(string operation, params string[] args)
    {
        if (args != null && args.Length > 0)
        {
            switch (operation)
            {
                case "move-forward":
                    MoveForward(args[0].ToFloat());
                    break;
                case "move-backward":
                    MoveBackward(args[0].ToFloat());
                    break;
                case "turn-right":
                    TurnRight(args[0].ToFloat());
                    break;
                case "turn-left":
                    TurnLeft(args[0].ToFloat());
                    break;
            }
        }        
    }

    private Vector3 ProcessMoveCommand(RobotMoveCommand moveCommand, float delta, Vector3 thisMovement)
    {
        if (moveCommand != null)
        {
            if (moveCommand.movedDistance < moveCommand.distance)
            {
                float t = delta * speed;                
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

                    moveCommand.finished = true;                    
                }
                else if (moveCommand.movedDistance == moveCommand.distance)
                {
                    moveCommand.finished = true;
                }               
            }
            else
            {
                moveCommand.finished = true;
            }
        }

        return thisMovement;
    }

    private void ProcessTurnCommand(RobotTurnCommand turnCommand, float delta)
    {
        if (turnCommand != null)
        {
            float thisRotation = 0f;

            if (turnCommand.progress < turnCommand.angle)
            {
                thisRotation = delta * rotationalSpeed;
                turnCommand.progress += thisRotation;

                if (turnCommand.progress > turnCommand.angle)
                {
                    float diff = turnCommand.progress - turnCommand.angle;
                    thisRotation -= diff;

                    turnCommand.finished = true;
                }
                else if (turnCommand.progress == turnCommand.angle)
                {
                    turnCommand.finished = true;
                }
            }
            else
            {
                turnCommand.finished = true;
            }

            Rotate(turnCommand.axis, thisRotation);
        }        
    }

}