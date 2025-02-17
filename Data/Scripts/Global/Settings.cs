using Godot;
using System;
using System.Collections.Generic;

public class Settings
{
    public int GridSize { get; private set; } = 32;
    public bool CutScene { get; set; } = false;
    public string CurrentSave { get; set; }
    public Vector2 Position { get; set; }
    public List<Save> Saves { get; set; }
    public GameSettings GameSettings { get; set; } = new GameSettings();
    public PlayerSettings PlayerSettings { get; set; } = new PlayerSettings();
}
