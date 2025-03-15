using Godot;
using System;

public partial class Bullet : CharacterBody2D
{
    private const float SPEED = 2500;
	private Vector2 _direction;
	public int Dmg { get; set; }

	public Bullet(Vector2 direction, Vector2 startPosition, int dmg, Node2D body)
	{
		CollisionMask = 4;
		_direction = direction;
		Dmg = dmg;
		GlobalPosition = startPosition;
		TopLevel = true;
        AddCollisionExceptionWith(body);
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
		AddChild(new PointLight2D()
		{
			Texture = GD.Load<Texture2D>("res://Data/Textures/DifusinLight.png"),
			Energy = 0.1f,
			Scale = new Vector2(0.1f, 0.1f),
			LightMask = 1,
			ShadowEnabled = true,
		});
    }

    public override void _PhysicsProcess(double delta)
    {
		var information = MoveAndCollide(_direction * SPEED * (float)delta);
		if (information != null)
		{
			if (information.GetCollider() is Enemy enemy)
				enemy.TakeDamage(Dmg);
			else if (information.GetCollider() is Player player)
				player.TakeDamage(Dmg);
			QueueFree();
		}
    }
}
