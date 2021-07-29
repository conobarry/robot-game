using System;
using Godot;

public class PauseMenu : PopupPanel
{

    private MainMenuButton resumeButton;

    private MainMenuButton restartButton;

    private MainMenuButton optionsButton;
    
    private MainMenuButton exitButton;

    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.resumeButton = (MainMenuButton)FindNode("ResumeButton");
        this.restartButton = (MainMenuButton)FindNode("RestartButton");
        this.optionsButton = (MainMenuButton)FindNode("OptionsButton");
        this.exitButton = (MainMenuButton)FindNode("ExitButton");

        resumeButton.ButtonPressed += ResumeButtonPressed;
        restartButton.ButtonPressed += RestartButtonPressed;
        exitButton.ButtonPressed += ExitButtonPressed;
    }

    public override void _Process(float delta)
    {
        // if (this.Visible)
        // {
        //     GD.Print("visible");
        //     if (Input.IsActionJustPressed("pause") || Input.IsActionJustPressed("ui_back"))
        //     {
        //         this.Hide();
        //         GetTree().Paused = false;
        //     }
        // }
        

        base._Process(delta);
    }

    private void ResumeButtonPressed(object sender, EventArgs args)
    {
        this.Hide();
        GetTree().Paused = false;
    }

    private void RestartButtonPressed(object sender, EventArgs args)
    {
        GetTree().ChangeScene("res://levels/test_level/test_level.tscn");
        GetTree().Paused = false;
    }

    private void OptionsButtonPressed(object sender, EventArgs args)
    {
        
    }

    private void ExitButtonPressed(object sender, EventArgs args)
    {
        GetTree().Paused = false;
        GetTree().ChangeScene("res://assets/ui/menus/main_menu.tscn");
    }

}
