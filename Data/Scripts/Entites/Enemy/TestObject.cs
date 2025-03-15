using Godot;
using System;

public partial class TestObject : CharacterBody2D
{
    public TestObject(Node2D body, uint layer, float size = 3)
    {
        CollisionMask = layer;
        AddCollisionExceptionWith(Global.SceneObjects.Player);
        AddCollisionExceptionWith(body);
        AddChild(new CollisionShape2D()
        {
            Shape = new CircleShape2D
            {
                Radius = size
            },
        });
    }

    public bool TestShot() =>
        TestMove(GetGlobalTransform(), GlobalPosition.DirectionTo(Global.SceneObjects.Player.GlobalPosition) * (GlobalPosition.DistanceTo(Global.SceneObjects.Player.GlobalPosition)));
}
