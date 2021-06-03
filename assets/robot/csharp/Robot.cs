using Godot;

public class Robot : KinematicBody {
    public void MoveForward(int distance)
    {
        GD.Print("Moving forward ", distance);
        // this.Visible = false;
        Vector3 direction = new Vector3(5, 0, 0);
        MoveAndCollide(direction);
        Translate(direction);
        // this.Translate(direction);
    }
    
    public void MoveBackward(int distance)
    {
        GD.Print("Moving backward ", distance);
    }
}