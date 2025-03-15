using Godot;
using System;

public partial class Shelter : Area2D
{
    public bool IsOccupied { get; set; }
    public bool IsPlayerNear { get; set; }

    public void OnBodyEntered(Node2D node)
    {
        if (node is Player)
            IsPlayerNear = true;
    }

    public void OnBodyExited(Node2D node)
    {
        if (node is Player)
            IsPlayerNear = false;
    }
}
