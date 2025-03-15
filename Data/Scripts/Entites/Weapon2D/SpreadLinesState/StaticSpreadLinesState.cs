using Godot;
using System;

public partial class StaticSpreadLinesState : Node, ISpreadLinesState
{
    public void Hide(SpreadLines spreadLines) =>
        spreadLines.State = new DisappearanceSpreadLinesState(spreadLines);

    public void Show(SpreadLines spreadLines) =>
        spreadLines.State = new AppearanceSpreadLinesState(spreadLines);

}
