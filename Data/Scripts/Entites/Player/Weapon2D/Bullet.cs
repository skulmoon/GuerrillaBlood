using Godot;
using System;
using static Godot.TextServer;

public partial class Bullet : CharacterBody2D
{
    private const float SPEED = 3000;
	private Vector2 _direction;
	public int Dmg { get; set; }

	public Bullet(Vector2 direction, Vector2 startPosition, int dmg)
	{
		CollisionLayer = 6;
		CollisionMask = 6;
		_direction = direction;
		Dmg = dmg;
		GlobalPosition = startPosition;
		TopLevel = true;
        AddCollisionExceptionWith(Global.SceneObjects.Player);
        AddChild(new CollisionShape2D()
		{
			Shape = new CircleShape2D
			{
				Radius = 5
			},
		});
		AddChild(new Sprite2D
		{
			Texture = GD.Load<Texture2D>("res://Data/Textures/Bullet.png"),
			Scale = new Vector2(2, 2),
		});
		Timer timer = new Timer()
		{
			Autostart = true,
			WaitTime = 4,
		};
		timer.Timeout += () => { QueueFree(); };
        AddChild(timer);
    }

    public override void _PhysicsProcess(double delta)
    {
       if (MoveAndCollide(_direction * SPEED * (float)delta) != null)
			QueueFree();
    }
}
