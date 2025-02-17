using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public static class ItemTypeExtension
{
    public static List<Item> GetList(this ItemType type)
    {
        return type switch
        {
            ItemType.Item => Global.SceneObjects.Player.Inventory.Items,
            ItemType.Armor => new List<Item>() { Global.SceneObjects.Player.Inventory.Armor },
            ItemType.Weapon => Global.SceneObjects.Player.Inventory.Weapons,
            _ => null,
        };
    }

    public static Type GetClass(this ItemType type)
    {
        return type switch
        {
            ItemType.Item => typeof(Item),
            ItemType.Armor => typeof(Armor),
            ItemType.Weapon => typeof(Weapon),
            _ => null,
        };
    }
}
