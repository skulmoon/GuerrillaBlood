using Godot;
using System;
using System.Collections.Generic;

public class GameSettings
{
    public int SaveNumber { get; set; }
    public string CurrentLocation { get; set; } = "Test/Test";

    public List<(int, bool)> MissionsData = new List<(int, bool)>();
}
