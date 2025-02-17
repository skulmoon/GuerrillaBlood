using Godot;
using System;

[GlobalClass]
public partial class Weapon : Item
{
    [Export] public string WeaponType { get; set; }
    [Export] public int Damage { get; set; }
    [Export] public int Speed { get; set; }
    [Export] public float TimeReload { get; set; }
    [Export] public int Ammunition { get; set; }
    [Export] public float SPM { get; set; }
    [Export] public float Spread { get; set; }

    public Weapon() : base() { }

    public Weapon(int id, int maxCount, string itemName, string description, string weaponType, int damage, int speed, float timeReload, int ammunition, float spm, float spread) : base(id, maxCount, itemName, description)
    {
        WeaponType = weaponType;
        Damage = damage;
        Speed = speed;
        TimeReload = timeReload;
        Ammunition = ammunition;
        SPM = spm;
        Spread = spread;
    }

    public override object Clone()
    {
        Item item = new Weapon(ID, MaxCount, Name, Description, WeaponType, Damage, Speed, TimeReload, Ammunition, SPM, Spread);
        item.Count = Count;
        return item;
    }
}