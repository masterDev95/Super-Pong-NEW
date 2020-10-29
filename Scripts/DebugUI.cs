using Godot;
using System;

public class DebugUI : Node2D
{
    public override void _Process(float delta)
    {
        string fps = Convert.ToString(Performance.GetMonitor(Performance.Monitor.TimeFps));
        GetNode<Label>("FPSLabel").Text = $"{fps} FPS";

        if (Input.IsActionJustPressed("debug_ui_toggle"))
            Visible = !Visible;

        if (Input.IsActionJustPressed("debug_vsync_toggle"))
            OS.VsyncEnabled = !OS.VsyncEnabled;
    }
}
