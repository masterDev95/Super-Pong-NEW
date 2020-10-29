using Godot;
using System;

public class UIButtonManager : Node
{
    public override void _Process(float delta)
    {
		if (Input.IsActionJustPressed("fullscreen_toggle"))
			OS.WindowFullscreen = !OS.WindowFullscreen;
			
		if (Input.IsActionJustPressed("restart"))
			GetTree().ReloadCurrentScene();
    }
}
