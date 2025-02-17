using Godot;
using System;
using System.Security.Cryptography;

public partial class Weapon2D : Node2D
{
    public int Damage { get; set; }
    public float Speed { get; set; }
    public float TimeReload { get; set; }
    public int Ammunition { get; set; }
    public int CurrentAmmunition { get; set; }
    public Timer FireRate { get; set; } = new Timer();
    public Timer Reload { get; set; } = new Timer();
    public float Spread { get; set; }
    public virtual string ShotSound { get => ""; }

    public Weapon2D(int damage, float speed, int timeReload, int ammunition, float spm, float spread)
    {
        AddChild(FireRate);
        AddChild(Reload);
        Damage = damage;
        Speed = speed;
        TimeReload = timeReload;
        Ammunition = ammunition;
        CurrentAmmunition = ammunition;
        FireRate.Autostart = true;
        FireRate.OneShot = true;
        FireRate.WaitTime = 60 / spm;
        Reload.Autostart = true;
        Reload.OneShot = true;
        Reload.WaitTime = timeReload;
        Spread = spread;
        Position = new Vector2(0, -32);
    }

    public void Attack()
    {
        if (Reload.TimeLeft == 0 && FireRate.TimeLeft == 0 && CurrentAmmunition > 0)
        {
            Bullet bullet = new Bullet(Position.DirectionTo(GetLocalMousePosition()), GlobalPosition, Damage);
            AddChild(bullet);
            FireRate.Start();
            Global.Sound.PlaySound(ShotSound);
            CurrentAmmunition--;
        }
    }
}
