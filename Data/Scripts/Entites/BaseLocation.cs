using Godot;
using System;

public partial class BaseLocation : Node2D
{
	public override void _Ready()
	{
		if (Global.Settings.IsNewButtonPressed)
		{
			Global.Settings.IsNewButtonPressed = false;
			Global.CutSceneManager.OutputCutScene(1, 1);
			Global.SceneObjects.Player.Inventory.TakeItem((Item)GD.Load<Item>("res://Data/Items/ActiveItems/Parabellum.tres").Duplicate());
        }
		Global.Sound.PlayMusic("1.mp3");
	}
}
