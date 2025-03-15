using Godot;
using System;
using System.Collections.Generic;

public class Settings
{
    private bool _isCutScene = false;

    public int GridSize { get; private set; } = 32;
    public bool IsNewButtonPressed { get; set; }
    public string CurrentSave { get; set; }
    public Vector2 Position { get; set; }
    public List<Save> Saves { get; set; }
    public GameSettings GameSettings { get; set; } = new GameSettings();
    public PlayerSettings PlayerSettings { get; set; } = new PlayerSettings();
    public bool CutScene 
    { 
        get => _isCutScene;
        set
        {
            _isCutScene = value;
            Global.SceneObjects.Storage.GetTree().Paused = _isCutScene;
        }
    }
}
