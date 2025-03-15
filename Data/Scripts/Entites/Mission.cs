using Godot;
using System;

public partial class Mission : Node
{
    [Export] public int Number { get; set; }
    [Export] public string Description { get; set; }
    [Export] public bool IsComplete { get; set; }
    
    public override void _Ready()
    {
        IsComplete = Global.Settings?.GameSettings?.MissionsData?.Find((x) => x.Item1 == Number).Item2 ?? false;
    }

    public void Start()
    {
        Global.SceneObjects.OnEnemyDie += Check;
        Global.SceneObjects.CurrentMission = this;
        var list = GetChildren();
        foreach (var item in list)
            ((EnemyArea)item).PlaceEnemy();
    }

    public void Check(Node enemy)
    {
        if (Global.SceneObjects.Enemies.Count <= 0)
            Complete();
    }

    public void Complete()
    {
        IsComplete = true;
        Global.SceneObjects.CurrentMission = null;
        Global.SceneObjects.OnEnemyDie -= Check;
        Global.Settings.GameSettings?.MissionsData?.Add((Number, true));
        if ((Global.Settings.GameSettings?.MissionsData?.Count ?? 0) >= 5)
            Global.SceneObjects.WinMenu.Open();
    }
}
