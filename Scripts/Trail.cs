using Godot;
using System;

public class Trail : Line2D
{
    public override void _Process(float delta)
    {
        Vector2 point = GlobalPosition;
        AddPoint(point);
    }
}
