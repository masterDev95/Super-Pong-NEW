using Godot;
using System;

public class Room1v1 : Node2D
{
	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("ui_pause"))
		{
			var pauseScreen = GetNode<Node2D>("PauseScreen");
			var cursor 		= pauseScreen.GetNode<Node2D>("Background/Cursor");

			pauseScreen.Visible = true;
			cursor.SetProcess(true);
			GetTree().Paused = true;
		}
	}
}
