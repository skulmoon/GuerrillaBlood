using Godot;
using System;

public partial class EnemyArea : Area2D
{
    [Export] public Vector2[] EnemyPositions { get; set; }

    public void PlaceEnemy()
    {
        Enemy enemy = Global.SceneObjects.EnemyFabric.Create(EnemyPositions);
        AddChild(enemy);
        Vector2 collisionSize = ((RectangleShape2D)GetNode<CollisionShape2D>("CollisionShape2D").Shape).Size;
        enemy.Position = new Vector2(
            (float)GD.RandRange(-collisionSize.X / 2, collisionSize.X / 2),
            (float)GD.RandRange(-collisionSize.Y / 2, collisionSize.Y / 2)
        );
    }
}
