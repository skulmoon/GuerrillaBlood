using Godot;
using System;

public partial class Armor : Item
{
    public string ArmorType { get; set; }

    public Armor(int id, int maxCount, string itemName, string description, string armorType) : base(id, maxCount, itemName, description)
    {
        ArmorType = armorType;
    }
    

}