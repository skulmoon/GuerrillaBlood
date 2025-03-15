using Godot;
using System.Collections.Generic;
using System.Diagnostics;

public abstract partial class Enemy : CharacterBody2D
{
    private float _health = 1200;
    private IEnemyState _state;
    private PlayerChecker _playerChecker;

    public List<Area2D> Triggers { get; set; } = new List<Area2D>();
    public Weapon2D Weapon { get; set; }
    public NavigationAgent2D Nav { get; set; }      
    public Player Player { get; set; }
    public Sprite2D Body { get; set; } = new Sprite2D();
    public Sprite2D Hat { get; set; } = new Sprite2D();
    public CollisionShape2D Collision { get; set; }
    public float Speed { get; private set; } = 200;
    public virtual float Health
    {
        get => _health;
        private set
        {
            if (value <= 0)
            {
                Global.SceneObjects.RemoveEnemy(this);
                QueueFree();
            }
            else
                _health = value;
        }
    }

    public IEnemyState State
    {
        get => _state;
        set
        {
            if (_state != null)
            {
                CallDeferred("remove_child", (Node)_state);
                ((Node)_state).QueueFree();
            }
            _state = value;
            if (value != null)
                CallDeferred("add_child", (Node)_state);
            GD.Print(value.GetType());
        }
    }

    public PlayerChecker PlayerChecker
    {
        get => _playerChecker;
        set
        {
            if (_playerChecker != null)
            {
                CallDeferred("remove_child", _playerChecker);
                _playerChecker.PlayerVisible -= OnPlayerVisible;
                _playerChecker.QueueFree();
            }
            _playerChecker = value;
            if (value != null)
            {
                CallDeferred("add_child", _playerChecker);
                _playerChecker.PlayerVisible += OnPlayerVisible;
            }
        }
    }

    public Enemy(float speed, int health, Weapon2D weapon)
    {
        ProcessMode = ProcessModeEnum.Inherit;
        Global.SceneObjects.Enemies.Add(this);
        PlayerChecker = new PlayerChecker(this);
        Nav = new NavigationAgent2D();
        AddChild(Nav);
        _health = health;
        Speed = speed;
        Weapon = weapon;
        AddChild(Weapon);
        CollisionLayer = 4;
        CollisionMask = 4;
        Collision = new CollisionShape2D()
        {
            Shape = new CircleShape2D()
            {
                Radius = 16
            }
        };
        AddChild(Collision);
        GD.Print(GetType());
        Body.Texture = GD.Load<Texture2D>("res://Data/Textures/Entities/Enemys/Enemybody.png");
        Hat.Texture = GD.Load<Texture2D>("res://Data/Textures/Entities/Enemys/EnemyHat.png");
        AddChild(Body);
        Body.AddChild(Hat);
        FloorBlockOnWall = false;
        FloorStopOnSlope = false;
        Global.SceneObjects.OnPlayerChanged += TakePlayer;
    }

    public void TakePlayer(Node player) =>
        Player = (Player)player;

    public virtual void MoveAndRotation(Vector2 position, float angel, double delta)
    {
        Move(position, delta);
        RotationSprite(angel, delta);
    }

    public virtual void MoveAndRotation(Vector2 position, double delta) =>
        RotationSprite(Move(position, delta).Angle(), delta);

    public virtual void RotationSprite(float rotation, double delta) =>
        Body.Rotation = Vector2.FromAngle(Body.Rotation).Lerp(Vector2.FromAngle(rotation), (float)delta * 10).Angle();

    public virtual Vector2 Move(Vector2 position, double delta)
    {
        Nav.TargetPosition = position;
        Vector2 direction = (Nav.GetNextPathPosition() - GlobalPosition).Normalized();
        Velocity = direction * (float)delta * Speed * 100;
        MoveAndSlide();
        return direction;
    }

    public virtual void Attack() =>
        Weapon.Attack(Body.Rotation, this);

    public virtual void TakeDamage(float damage) =>
        Health -= damage;

    public abstract void OnPlayerVisible();
}
