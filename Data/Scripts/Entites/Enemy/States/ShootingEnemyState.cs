using Godot;
using System;

public partial class ShootingEnemyState : Node, IEnemyState
{
    private Enemy _enemy;
    private PatrolPoint _basePosition;

    public ShootingEnemyState(Enemy enemy, PatrolPoint basePosition)
    {
        _enemy = enemy;
        _basePosition = basePosition;
    }

    public override void _PhysicsProcess(double delta)
    {
        _enemy.RotationSprite(_enemy.GlobalPosition.AngleToPoint(Global.SceneObjects.Player.GlobalPosition), delta);
        if (_enemy.PlayerChecker.IsPlayerVisible)
            _enemy.Attack();
    }

    public void MoveToPlayer() =>
        _enemy.State = new PersecutionEnemyState(_enemy, _basePosition);

    public void MoveFromPlayer() =>
        _enemy.State = new RetreatEnemyState(_enemy, _basePosition);
}
