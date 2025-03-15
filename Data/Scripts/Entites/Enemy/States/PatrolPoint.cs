using Godot;
using System;

public partial class PatrolPoint : Area2D
{
	public PatrolPoint(Vector2 position)
	{
		GlobalPosition = position;
		CollisionLayer = 256;
		AddChild(new CollisionShape2D
		{
			Shape = new CircleShape2D
			{
				Radius = 4,
			}
		});
	}
}
