using Godot;
using System;

public partial class DeathMenu : CanvasLayer
{
    public override void _Ready()
    {
        Global.SceneObjects.DeathMenu = this;
    }

    public void Open()
    {
        Visible = !Visible;
        Global.Settings.CutScene = Visible;
        Global.SceneObjects.CurrentMission = null;
    }
}
