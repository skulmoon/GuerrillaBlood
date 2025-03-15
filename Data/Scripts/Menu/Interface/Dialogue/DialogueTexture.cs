using Godot;
using System;

public partial class DialogueTexture : TextureRect
{
	public override void _Ready()
	{
		Global.SceneObjects.DialogueTexture = this;
    }
}
