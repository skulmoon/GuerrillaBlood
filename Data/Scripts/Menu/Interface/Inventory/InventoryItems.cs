using Godot;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class InventoryItems : Control
{
	private PlayerInventory _playerInventory;
	private int lineCount = 0;
    public List<Cell> Cells { get; private set; } = new List<Cell>();
    [Export] public int SellInLine { get; set; } = 6;
    [Export] public ItemType Type { get; set; } = ItemType.Item;

    public override void _Ready()
    {
        Global.SceneObjects.OnPlayerChanged += TakePlayer;
    }

    public override void _ExitTree() =>
        Global.SceneObjects.OnPlayerChanged -= TakePlayer;

    public void TakePlayer(Node player)
    {
        _playerInventory = ((Player)player).Inventory;
        ShowInventory();
    }

    public void ShowInventory()
    {
        if (Type == ItemType.Item)
        {
            AddCells();
            AddCells();
        }
        else
        {
            AddCells();
        }
    }

    public float AddCells()
    {
        float cellSize = Size.X / SellInLine;
        for (int i = 0; i < SellInLine; i++)
        {
            Cell cell = new Cell(new Vector2(i * cellSize, lineCount * cellSize), new Vector2(cellSize, cellSize), this, i + (lineCount * SellInLine));
            Label label = new Label
            {
                Text = (i + lineCount * SellInLine).ToString()
            };
            cell.AddChild(label);
            AddChild(cell);
            Cells.Add(cell);
        }
        lineCount++;
        return cellSize;
    }
}
