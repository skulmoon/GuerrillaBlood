using Godot;
using System;

public partial class PersecutionEnemyState : Node2D, IEnemyState
{
    private Enemy _enemy;
    private PatrolPoint _basePosition;

    public PersecutionEnemyState(Enemy enemy, PatrolPoint basePosition)
    {
        _enemy = enemy;
        _basePosition = basePosition;
    }

    public override void _PhysicsProcess(double delta)
    {
        _enemy.MoveAndRotation(Global.SceneObjects.Player.GlobalPosition, delta);
    }

    public void ShootingToPlayer() =>
        _enemy.State = new ShootingEnemyState(_enemy, _basePosition);
}
