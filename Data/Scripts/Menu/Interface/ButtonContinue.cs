using Godot;
using System;

public partial class ButtonContinue : TextureButton
{
	[Export] public PauseMenu Menu { get; set; }

    public ButtonContinue()
    {
        ProcessMode = ProcessModeEnum.WhenPaused;
    }

    public void OnPressed()
	{
        Menu.Visible = false;
        Global.Settings.CutScene = false;
    }
}
