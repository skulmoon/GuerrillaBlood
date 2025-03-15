using Godot;
using System;

public partial class WinMenu : CanvasLayer
{
	public override void _Ready()
	{
		Global.SceneObjects.WinMenu = this;
	}

	public void Open()
	{
        Visible = !Visible;
        Global.Settings.CutScene = Visible;
    }
}
