using Godot;
using System;

public partial class HpProgressBar : ProgressBar
{
    public HpProgressBar()
    {
        Global.SceneObjects.Hp = this;
    }
}
