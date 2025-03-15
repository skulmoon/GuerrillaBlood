using Godot;
using System;

public partial class PlayerCamera : Camera2D
{
    public float Aim { get; set; } = 0;
    public override void _Process(double delta)
	{
        GlobalPosition = GlobalPosition.Lerp(Global.SceneObjects.Player.GlobalPosition.Lerp(GetGlobalMousePosition(), 0.1f + Aim), 10 * (float)delta);
    }
}
