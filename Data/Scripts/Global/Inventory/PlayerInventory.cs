using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PlayerInventory : Node
{
    public List<Item> Items { get; set; } = new List<Item>(new Item[12]);
    public List<Item> ActiveItems { get; set; } = new List<Item>(new Item[3]);
    public Item Armor { get; set; }
    public int Scruples { get; set; } = 1000;

    public event Action ArmorChanged = () => { };

    public PlayerInventory()
    {
        Items[0] = (Item)GD.Load<Item>("res://Data/Items/ActiveItems/Parabellum.tres").Duplicate();
    }

    public bool TakeItem(Item item)
	{
        InventoryItems inventoryItems = Global.SceneObjects.InventoryMenu.GetNode<InventoryItems>("Items");
        int? freeNumber = null;
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i]?.ID == item.ID) 
            {
                item.Count = Items[i].Staked(item.Count);
                if (item.Count == 0)
                {
                    inventoryItems.Cells.Find(x => x.Item == Items[i]).UpdateItem();
                    return true;
                }
            }
            else if ((freeNumber == null) && (Items[i] == null))
                freeNumber = i;
        }
        if (freeNumber != null)
        {
            Items[(int)freeNumber] = item;
            inventoryItems.Cells.Find(x => x.ItemNumber == (int)freeNumber).UpdateItem();
            return true;
        }
        return false;
    }

    public void MovingItem(ItemType startType, ItemType endType, int startPosition, int endPosition)
    {
        List<Item> startList = startType.GetList();
        List<Item> endList = endType.GetList();
        (endList[endPosition], startList[startPosition]) = (startList[startPosition], endList[endPosition]);
    }
}
