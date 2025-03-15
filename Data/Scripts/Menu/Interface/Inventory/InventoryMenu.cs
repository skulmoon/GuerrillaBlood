using Godot;
using System;

public partial class InventoryMenu : Control
{
    public InventoryMenu() =>
        Global.SceneObjects.InventoryMenu = this;

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("open_inventory"))
        {
            Visible = !Visible;
        }
    }

    public void ShowInventory()
    {
        Visible = true;
        Cell.ActiveWeaponCells.ForEach(cell => cell.Visible = false);
    }
}
