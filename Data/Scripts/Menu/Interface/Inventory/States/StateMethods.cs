using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Godot;

partial class StateCellMethods : Node
{
    static public void ReleaseOne(Cell cell)
    {
        if (Cell.EnteredMouseCell != null)
        {
            if ((Cell.EnteredMouseCell?.Item?.ID) == (cell?.Item?.ID) && Cell.EnteredMouseCell?.Item != null)
            {
                if (Cell.EnteredMouseCell?.Item.Count < Cell.EnteredMouseCell?.Item.MaxCount)
                {
                    Cell.EnteredMouseCell.Item.Count++;
                    DecrementItem(cell);
                    Cell.EnteredMouseCell.UpdateItem();
                    cell.UpdateItem();
                }
            }
            else if (Cell.EnteredMouseCell.Item == null && cell?.Item != null)
            {
                Global.SceneObjects.Player.Inventory.Items[Cell.EnteredMouseCell.ItemNumber] = (Item)cell.Item.Clone();
                Global.SceneObjects.Player.Inventory.Items[Cell.EnteredMouseCell.ItemNumber].Count = 1;
                DecrementItem(cell);
                Cell.EnteredMouseCell.UpdateItem();
                cell.UpdateItem();
            }
        }
    }

    static void DecrementItem(Cell cell)
    {
        if (cell.Item.Count > 1)
            cell.Item.Count--;
        else if (cell.Item.Count <= 1)
        {
            cell.Item = null;
            Global.SceneObjects.Player.Inventory.Items[cell.ItemNumber] = null;
            cell.TopLevel = false;
            cell.GlobalPosition = cell.GetViewport().GetMousePosition() - (cell.Size / 2);
            cell.State = new TeleportationCellState(cell);
        }
    }

    static public void ReleasedActiveShard(Cell cell)
    {
        bool isMainTake = false;
        Cell ActiveCell = null;
        Cell changeCell = null;
        Cell.ActiveWeaponCells.ForEach(x =>
        {
            if (x == cell || x == Cell.EnteredMouseCell)
            {
                if (x == Cell.ActiveWeaponCells[0])
                    isMainTake = true;
                if (ActiveCell == null)
                {
                    if (x == cell)
                    {
                        changeCell = Cell.EnteredMouseCell;
                        ActiveCell = cell;
                    }
                    else
                    {
                        changeCell = cell;
                        ActiveCell = Cell.EnteredMouseCell;
                    }
                }
            }
        });
        bool isVoid = false;
        isVoid = !Cell.ActiveWeaponCells.Exists(x => x != Cell.ActiveWeaponCells[0] && ((x != ActiveCell && x.Item != null) || (x == ActiveCell && changeCell.Item != null)));
        GD.Print("MainTake:", isMainTake);
        GD.Print("Void:", isVoid);
        GD.Print(ActiveCell == null, " ", changeCell.Item == null);
        GD.Print("------------");
        if (isVoid)
            Cell.ActiveWeaponCells[0].Disabled = false;
        else
            Cell.ActiveWeaponCells[0].Disabled = true;
        if ((isMainTake && changeCell.Item != null) || (!isMainTake && Cell.ActiveWeaponCells[0].Item != null))
            for (int i = 1; i < Cell.ActiveWeaponCells.Count; i++)
                Cell.ActiveWeaponCells[i].Disabled = false;
        else
            for (int i = 1; i < Cell.ActiveWeaponCells.Count; i++)
                Cell.ActiveWeaponCells[i].Disabled = true;
    }

    static public void CheckActiveShards()
    {
        if (!Cell.ActiveWeaponCells.Exists(x => x != Cell.ActiveWeaponCells[0] && x.Item != null))
            Cell.ActiveWeaponCells[0].Disabled = false;
        else
            Cell.ActiveWeaponCells[0].Disabled = true;
        if (Cell.ActiveWeaponCells[0].Item != null)
            for (int i = 1; i < Cell.ActiveWeaponCells.Count; i++)
                Cell.ActiveWeaponCells[i].Disabled = false;
        else
            for (int i = 1; i < Cell.ActiveWeaponCells.Count; i++)
                Cell.ActiveWeaponCells[i].Disabled = true;
    }
}

