using Godot;
using System;

public partial class ButSave : TextureButton
{
    [Export] public Label Label { get; set; }

    public ButSave()
    {
        ProcessMode = ProcessModeEnum.WhenPaused;
    }

    public void OnPressed()
	{
        if (Global.SceneObjects.CurrentMission == null)
        {
            Global.JSON.SaveGame();
        }
        else
        {
            Label.Text = "Сейчас запущенна миссия, сохранение невозможно.";
        }
    }
}
