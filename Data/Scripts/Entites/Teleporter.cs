using Godot;
using Microsoft.VisualBasic;
using System;

public partial class Teleporter : Area2D, IInteractionArea
{
	[Export] public Teleporter ConnectedTeleporter { get; set; }
    [Export] public Mission Mission { get; set; }
    [Export] public RichTextLabel Label { get; set; }

    public override void _Ready()
    {
        AreaEntered += (area) =>
        {
            if (Global.SceneObjects.CurrentMission == null)
            {
                if (Mission != null)
                {
                    if (!Mission.IsComplete)
                        Label.Text = $"Миссия:\n{Mission.Description}";
                    else
                        Label.Text = "Миссия выполнена!";
                }
                else
                    Label.Text = string.Empty;
            }
            else
                Label.Text = "Выполните текущую миссию";
        };
        AreaExited += (area) =>
        {
            Label.Text = string.Empty;
        };
    }

    public void Interaction()
    {
        if (Global.SceneObjects.CurrentMission == null)
        {
            Global.JSON.SaveGame();
            Global.SceneObjects.Player.GlobalPosition = ConnectedTeleporter.GlobalPosition;
            if (Mission != null && !Mission.IsComplete)
                Mission.Start();
        }
    }
}
