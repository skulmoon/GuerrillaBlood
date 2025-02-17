using Godot;
using System;

public partial class CloseButton : TextureButton
{
	public void OnPressed()
	{
		Global.JSON.SaveGame();
		GetTree().ChangeSceneToFile("res://Data/Scenes/Menu/MainMenu.tscn");
	}
}
