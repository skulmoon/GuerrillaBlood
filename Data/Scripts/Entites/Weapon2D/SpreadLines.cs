using Godot;
using System;
using System.Collections.Generic;

public partial class SpreadLines : Node2D
{
	private List<Sprite2D> _lines = new List<Sprite2D>();
	private ISpreadLinesState _state;

    public ISpreadLinesState State
	{
		get => _state;
		set
		{
			RemoveChild((Node)_state);
			_state = value;
			AddChild((Node)_state);
		}
	}

	public new void Hide() =>
		State.Hide(this);

    public new void Show() =>
        State.Show(this);

    public SpreadLines()
	{
        _state = new StaticSpreadLinesState();
		AddChild((Node)_state);
        for (int i = 0; i < 2; i++)
        {
            _lines.Add(new Sprite2D()
            {
                Texture = (Texture2D)GD.Load("res://Data/Textures/Entities/Line.png").Duplicate(),
                Scale = new Vector2(0.5f, 0.5f),
            });
            AddChild(_lines[i]);
        }
    }

	public void SetLinesRotation(float spriteRotation, float spread, double delta)
	{
		_lines[0].Rotation = Vector2.FromAngle(spriteRotation + spread / 2 - Mathf.DegToRad(90)).Lerp(Vector2.FromAngle(_lines[0].Rotation), 5 * (float)delta).Angle();
		_lines[1].Rotation = Vector2.FromAngle(spriteRotation - spread / 2 - Mathf.DegToRad(90)).Lerp(Vector2.FromAngle(_lines[1].Rotation), 5 * (float)delta).Angle();
        _lines.ForEach((x) => x.Position = Vector2.FromAngle(x.Rotation + Mathf.DegToRad(90)) * 150);
    }
}
