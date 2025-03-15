using Godot;
using System;

public partial class AmunitionProgressBar : ProgressBar
{
	public AmunitionProgressBar()
	{
		Global.SceneObjects.Ammunition = this;
	}
}
