using Godot;
using System;

public partial class RetreatEnemyState : Node2D, IEnemyState
{
    private Enemy _enemy;
    private PatrolPoint _basePosition;

    public RetreatEnemyState(Enemy enemy, PatrolPoint basePosition)
    {
        _enemy = enemy;
        _basePosition = basePosition;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 playerPosition = Global.SceneObjects.Player.GlobalPosition;
        _enemy.MoveAndRotation(playerPosition + playerPosition.DirectionTo(_enemy.GlobalPosition) * 250, GetAngleTo(Global.SceneObjects.Player.GlobalPosition), delta);
        if (_enemy.PlayerChecker.IsPlayerVisible)
            _enemy.Attack();
    }
    public void ShootingToPlayer() =>
        _enemy.State = new ShootingEnemyState(_enemy, _basePosition);
}
