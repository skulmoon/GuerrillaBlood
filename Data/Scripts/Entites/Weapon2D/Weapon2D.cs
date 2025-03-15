using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

public partial class Weapon2D : Node2D, IActiveItem
{
    private RandomNumberGenerator _random = new RandomNumberGenerator();
    private float _speed;
    private float _saveSpread;
    private float _testBullet;
    private int _currentAmmunition;
    private Node2D _currentBody;

    public bool ActiveSecondAction { get; set; } = false;
    public int Damage { get; set; }
    public float SpeedMultiplier { get; set; }
    public float TimeReload { get; set; }
    public int Ammunition { get; set; }
    public int CurrentAmmunition { 
        get => _currentAmmunition; 
        set
        {
            _currentAmmunition = value;
            if (_currentBody != null && _currentBody is Player)
                Global.SceneObjects.Ammunition.Value = value;
        }
    }
    public Timer FireRate { get; set; } = new Timer();
    public Timer Reload { get; set; } = new Timer();
    public float Spread { get; set; }
    public float CurrentSpread { get; set; }
    public float Stabilization { get; set; }
    public int Multishot { get; set; }
    public float Aim { get; set; }
    public virtual string ShotSound { get => "Weapon/BaseShot.mp3"; }
    public virtual string ReloadSound { get => "Weapon/BaseSound.mp3"; }

    public Weapon2D(int damage, float speedMultiplier, int timeReload, int ammunition, float spm, float spread, float stabilization, int multishot, float aim)
    {
        AddChild(FireRate);
        AddChild(Reload);
        Damage = damage;
        SpeedMultiplier = speedMultiplier;
        TimeReload = timeReload;
        Ammunition = ammunition;
        CurrentAmmunition = ammunition;
        FireRate.Autostart = true;
        FireRate.OneShot = true;
        FireRate.WaitTime = 60 / spm;
        Reload.Autostart = true;
        Reload.OneShot = true;
        Reload.WaitTime = timeReload;
        Reload.Timeout += () =>
        {
            CurrentAmmunition = Ammunition;
        };
        Spread = Mathf.DegToRad(spread);
        CurrentSpread = Spread;
        Stabilization = stabilization;
        Multishot = multishot;
        Aim = aim;
    }

    public void Attack(float angle, Node2D body)
    {
        if(Reload.TimeLeft == 0)
        {
            if (FireRate.TimeLeft == 0 && CurrentAmmunition > 0)
            {
                float angleDiffusion;
                for (int i = 0; i < Multishot; i++)
                {
                    angleDiffusion = _random.RandfRange(-CurrentSpread / 2, CurrentSpread / 2);
                    AddChild(new Bullet(Vector2.FromAngle(angle).Rotated(angleDiffusion), GlobalPosition, Damage, body));
                }
                CurrentSpread += Mathf.DegToRad(20);
                FireRate.Start();
                Global.Sound.PlaySound(ShotSound);
                _currentBody = body;
                CurrentAmmunition--;
            }
            else if (CurrentAmmunition <= 0)
            {
                Global.Sound.PlaySound(ReloadSound, 0.1f);
                Reload.Start();
            }
        }
    }

    public void Aiming()
    {
        Player player = Global.SceneObjects.Player;
        if (ActiveSecondAction)
        {
            player.Speed = _speed;
            Spread = _saveSpread;
            player.GetNode<PlayerCamera>("Camera2D").Aim = 0;
        }
        else
        {
            _speed = player.Speed;
            player.Speed /= (3 / Stabilization) + 1;
            _saveSpread = Spread;
            Spread /= (Aim + 0.1f) * 10;
            player.GetNode<PlayerCamera>("Camera2D").Aim = Aim;
        }
        ActiveSecondAction = !ActiveSecondAction;
    }

    public override void _PhysicsProcess(double delta)
    {
        CurrentSpread = Mathf.Lerp(CurrentSpread, Spread, Stabilization * (float)delta * 1.5f);
    }

    public void FirstAction(params object[] arguments) =>
        Attack((float)arguments[0], (Node2D)arguments[1]);
    
    public void SecondAction() => 
        Aiming();

    public List<object> GetParameters() =>
        new List<object> { CurrentAmmunition };

    public void SetParameters(List<object> list) =>
        CurrentAmmunition = (int)list[0];
}
