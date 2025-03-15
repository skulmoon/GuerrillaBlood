using Godot;
using System;

public partial class ButtonExit : TextureButton
{
	public void OnPressed()
	{
		GetTree().Quit();
	}
}
