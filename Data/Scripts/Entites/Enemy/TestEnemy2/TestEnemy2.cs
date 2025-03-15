using Godot;
using System;
using System.Collections.Generic;

public partial class TestEnemy2 : Enemy
{
    private List<Enemy> _enemiesNearby = new List<Enemy>();

    public TestEnemy2(float speed, int health, Vector2[] positions) : base(speed, health, new Parabellum())
    {
        Area2D area = new Area2D { CollisionMask = 4 };
        area.AddChild(new CollisionShape2D { Shape = new CircleSectorShape2D(60, 800) });
        Triggers.Add(area);
        AddChild(area);
        area.BodyEntered += (body) =>
        {
            if (body is Player player)
            {
                PlayerChecker.IsPlayerInArea = true;
                GD.Print($"{player.Name}");
            }
        };
        area.BodyExited += (body) =>
        {
            if (body is Player && PlayerChecker != null)
                PlayerChecker.IsPlayerInArea = false;
        };
        area = new Area2D { CollisionMask = 4 };
        area.AddChild(new CollisionShape2D { Shape = new CircleShape2D { Radius = 200 } });
        Triggers.Add(area);
        AddChild(area);
        area.BodyEntered += (body) => 
        {
            if (body is Player)
                State.MoveFromPlayer();
        };
        area.BodyExited += (body) =>
        {
            if (body is Player)
                State.ShootingToPlayer();
        };
        area = new Area2D { CollisionMask = 4 };
        area.AddChild(new CollisionShape2D { Shape = new CircleShape2D { Radius = 500 } });
        Triggers.Add(area);
        AddChild(area);
        area.BodyEntered += (body) =>
        {
            if (body is Enemy enemy)
            {
                _enemiesNearby.Add(enemy);
                if (State is not PatrolEnemyState)
                    enemy.OnPlayerVisible();
            }
            if (body is Player)
                State.ShootingToPlayer();
        };
        area.BodyExited += (body) =>
        {
            if (body is Enemy enemy)
                _enemiesNearby.Remove(enemy);
            if (body is Player)
                State.MoveToPlayer();
        };
        State = new PatrolEnemyState(this, 2, new LinkedList<Vector2>(positions));
    }

    public override void OnPlayerVisible()
    {
        if (State is PatrolEnemyState)
            State.NoticePlayer();
    }

    public override void TakeDamage(float damage)
    {
        GD.Print($"Enemy take {damage} damage.");
        base.TakeDamage(damage);
    }

    public override void RotationSprite(float angel, double delta)
    {
        base.RotationSprite(angel, delta);
        Triggers[0].Rotation = Body.Rotation;
    }
}
