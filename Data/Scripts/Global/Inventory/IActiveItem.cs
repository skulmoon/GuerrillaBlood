using Godot;
using System;
using System.Collections.Generic;

public interface IActiveItem
{
	public bool ActiveSecondAction { get; set; }
	public void FirstAction(params object[] arguments);
	public void SecondAction() { }
	public List<object> GetParameters();
	public void SetParameters(List<object> list);
}
