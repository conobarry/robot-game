using System;
using Godot;

public class MainMenu : Control
{

    private MainMenuButton playButton;

    private MainMenuButton optionsButton;
    
    private MainMenuButton quitButton;

    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.playButton = (MainMenuButton)FindNode("PlayButton");
        this.optionsButton = (MainMenuButton)FindNode("OptionsButton");
        this.quitButton = (MainMenuButton)FindNode("QuitButton");

        playButton.ButtonPressed += PlayButtonPressed;
    }

    private void PlayButtonPressed(object sender, EventArgs args)
    {
        GetTree().ChangeScene("res://levels/test_level/test_level.tscn");
    }

    private void OptionsButtonPressed(object sender, EventArgs args)
    {
        
    }

    private void QuitButtonPressed(object sender, EventArgs args)
    {
        GetTree().Quit();
    }

}
