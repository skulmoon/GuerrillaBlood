using Godot;
using System;

public partial class CloseButton : TextureButton
{
    private bool _isPressed = false;
    [Export] public Label Label { get; set; }
    [Export] public PauseMenu Menu { get; set; }

    public CloseButton()
    {
        ProcessMode = ProcessModeEnum.WhenPaused;
        VisibilityChanged += () => _isPressed = false;
    }

    public void OnPressed()
	{
        if (Global.Settings.GameSettings.MissionsData.Count >= 5 || Global.SceneObjects.Player.Health <= 0 ||_isPressed)
        {
            GetTree().ChangeSceneToFile("res://Data/Scenes/Menu/MainMenu.tscn");
            Global.Settings.CutScene = false;
            Global.Sound.PlayMusic("MainMenuMusic.mp3");
        }
        else if (Global.SceneObjects.CurrentMission == null)
        {
            Global.JSON.SaveGame();
            Menu.Visible = false;
		    GetTree().ChangeSceneToFile("res://Data/Scenes/Menu/MainMenu.tscn");
            Global.Settings.CutScene = false;
            Global.Sound.PlayMusic("MainMenuMusic.mp3");
        }
        else if (!_isPressed)
        {
            Label.Text = "Сейчас запущенна миссия, сохранение невозможно. Если всё равно хотите выйти, то нажмите ещё раз на кнопку.";
            _isPressed = true;
        }
	}
}
