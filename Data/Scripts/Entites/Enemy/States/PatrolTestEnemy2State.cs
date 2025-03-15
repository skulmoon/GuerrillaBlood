using Godot;
using System.Collections.Generic;

public partial class PatrolEnemyState : Node2D, IEnemyState
{
    private Enemy _enemy;
    private LinkedList<PatrolPoint> _positions = new LinkedList<PatrolPoint>();
    private LinkedListNode<PatrolPoint> _currentPosition;
    private Timer _breakTimer = new Timer();

    public PatrolEnemyState(Enemy enemy, float breakTime, IEnumerable<Vector2> positions)
    {
        _enemy = enemy;
        _breakTimer.Autostart = true;
        _breakTimer.OneShot = true;
        _breakTimer.WaitTime = breakTime;
        AddChild(_breakTimer);
        _breakTimer.Timeout += () => _currentPosition = _positions.First;
        Area2D area = new Area2D
        {
            CollisionMask = 256,
        };
        area.AddChild(new CollisionShape2D
        {
            Shape = new CircleShape2D
            {
                Radius = 4
            }
        });
        area.AreaEntered += OnPatrolPointEntered;
        AddChild(area);
        Ready += () =>
        {
            foreach (Vector2 point in positions)
            {
                PatrolPoint patrolPoint = new PatrolPoint(point);
                GetTree().CurrentScene.CallDeferred("add_child", patrolPoint);
                _positions.AddLast(patrolPoint);
            }
        };
    } 

    public void OnPatrolPointEntered(Area2D area)
    {
        if (area is PatrolPoint patrolPoint)
        {
            _currentPosition = _currentPosition?.Next ?? null;
            if (_currentPosition == null)
                _breakTimer.Start();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_currentPosition != null)
            _enemy.MoveAndRotation(_currentPosition.Value.GlobalPosition, (float)delta);
    }

    public void NoticePlayer()
    {
        Vector2 playerPosition = Global.SceneObjects.Player.GlobalPosition;
        if (_enemy.GlobalPosition.DistanceTo(playerPosition) < ((CircleShape2D)((CollisionShape2D)_enemy.Triggers[1].GetChildren()[0]).Shape).Radius)
            _enemy.State = new RetreatEnemyState(_enemy, _positions.Last.Value);
        else if (_enemy.GlobalPosition.DistanceTo(playerPosition) < ((CircleShape2D)((CollisionShape2D)_enemy.Triggers[2].GetChildren()[0]).Shape).Radius)
            _enemy.State = new ShootingEnemyState(_enemy, _positions.Last.Value);
        else
            _enemy.State = new PersecutionEnemyState(_enemy, _positions.Last.Value);
    }
}
