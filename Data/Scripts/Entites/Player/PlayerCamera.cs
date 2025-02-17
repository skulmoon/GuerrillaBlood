using Godot;
using System;

public partial class PlayerCamera : Camera2D
{
    public Vector2 CursorPosition { get => GetGlobalMousePosition(); }
    public override void _Process(double delta)
	{
        GlobalPosition = GlobalPosition.Lerp(Global.SceneObjects.Player.GlobalPosition.Lerp(CursorPosition, 0.1f), 10 * (float)delta);
    }
}
