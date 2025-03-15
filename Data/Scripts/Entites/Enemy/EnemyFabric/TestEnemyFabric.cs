using Godot;
using System;
using System.Collections.Generic;

public partial class TestEnemyFabric : EnemyFabric
{
    public TestEnemyFabric()
    {
        Global.SceneObjects.EnemyFabric = this;
    }

    public override Enemy Create(Vector2[] positions) =>
		new TestEnemy2(100, 50, positions);
}
