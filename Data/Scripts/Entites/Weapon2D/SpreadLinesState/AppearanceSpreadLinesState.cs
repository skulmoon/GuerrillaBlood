using Godot;
using System;

public partial class AppearanceSpreadLinesState : Node, ISpreadLinesState
{
    private SpreadLines _spreadLines;
    private int _speed = 10;

    public AppearanceSpreadLinesState(SpreadLines spreadLines) =>
        _spreadLines = spreadLines;

    public override void _Process(double delta)
    {
        delta *= _speed;
        if (_spreadLines.Modulate.A + (float)delta < 1)
        {
            Color color = _spreadLines.Modulate;
            color.A += (float)delta;
            _spreadLines.Modulate = color;
        }
        else
        {
            Color color = _spreadLines.Modulate;
            color.A = 1;
            _spreadLines.Modulate = color;
            _spreadLines.State = new StaticSpreadLinesState();
        }
    }

    public void Hide(SpreadLines spreadLines) =>
        spreadLines.State = new DisappearanceSpreadLinesState(spreadLines);
}
