using Godot;
using System;
using System.Collections.Generic;

public abstract partial class EnemyFabric : Node
{
    public abstract Enemy Create(Vector2[] positions);
}
