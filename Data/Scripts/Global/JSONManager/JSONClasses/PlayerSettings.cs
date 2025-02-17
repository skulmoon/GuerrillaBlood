using Godot;
using System;
using System.Collections.Generic;

public class PlayerSettings
{
    public List<Item> Items { get; set; } = new List<Item>();
    public Item Armor { get; set; }
    public List<Item> Weapons { get; set; } = new List<Item>();
    public int Scruples { get; set; }
    public Vector2 Position { get; set; } = new Vector2(16, 16);
}
