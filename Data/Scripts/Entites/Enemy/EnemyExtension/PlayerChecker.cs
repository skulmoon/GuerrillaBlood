using Godot;
using System;

public partial class PlayerChecker : Node2D
{
	private TestObject _testObject;
	private double _progress;
	public event Action PlayerVisible = () => { };
	public event Action PlayerInvisible = () => { };

    public bool IsPlayerInArea { get; set; }
	public bool IsPlayerVisible { get; set; }

    public PlayerChecker(Node2D enemy)
	{
        _testObject = new TestObject(enemy, 4);
        _progress = 0.1;
		AddChild(_testObject);
    }

    public override void _Process(double delta)
	{
		if (!_testObject.TestShot() && IsPlayerInArea)
		{
			if (_progress > 100)
				PlayerVisible.Invoke();
			IsPlayerVisible = true;
            _progress += delta * 300;
        }
		else
		{
			IsPlayerVisible = false;
			if (_progress < 0) 
				PlayerInvisible.Invoke();
			else
				_progress -= delta * 200;
        }
    }
}
