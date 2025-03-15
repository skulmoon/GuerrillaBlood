using Godot;
using System;

public partial class DisappearanceSpreadLinesState : Node, ISpreadLinesState
{
    private SpreadLines _spreadLines;
    private int _speed = 10;

    public DisappearanceSpreadLinesState(SpreadLines spreadLines) =>
        _spreadLines = spreadLines;

    public override void _Process(double delta)
    {
        delta *= _speed;
        if (_spreadLines.Modulate.A - (float)delta > 0)
        {
            Color color = _spreadLines.Modulate;
            color.A -= (float)delta;
            _spreadLines.Modulate = color;
        }
        else
        {
            Color color = _spreadLines.Modulate;
            color.A = 0;
            _spreadLines.Modulate = color;
            _spreadLines.State = new StaticSpreadLinesState();
        }
    }

    public void Show(SpreadLines spreadLines) =>
        spreadLines.State = new AppearanceSpreadLinesState(spreadLines);
}
