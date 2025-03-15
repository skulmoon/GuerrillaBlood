using Godot;
using System;
using System.Collections.Generic;

public partial class CircleSectorShape2D : ConvexPolygonShape2D
{
    public CircleSectorShape2D(float sector, float radius)
    {
        sector = Mathf.DegToRad(sector);
        List<Vector2> points = new List<Vector2>
        {
            new Vector2(0, 0)
        };
        for (float i = -sector / 2; i <= sector / 2; i += Mathf.DegToRad(15))
        {
            points.Add(Vector2.FromAngle(i) * radius);
            GD.Print(Vector2.FromAngle(i) * radius);
        }
        Points = points.ToArray();
    }
}
