using Godot;
using System;

public class Room1v1 : Node2D
{
	private void PaddleReposition()
	{
		KinematicBody2D p1 = GetNode<KinematicBody2D>("Player1");
		KinematicBody2D p2 = GetNode<KinematicBody2D>("Player2");

		float yPos		= OS.GetScreenSize().y / 2;
		float p2XPos	= OS.GetScreenSize().x - 64;

		p1.Position = new Vector2(p1.Position.x, yPos);
		p2.Position = new Vector2(p2XPos, yPos);
	}

	private void BallReposition()
	{
		KinematicBody2D ball = GetNode<KinematicBody2D>("Ball");
		ball.Position = OS.GetScreenSize() / 2;
	}

	public override void _Ready()
	{
		PaddleReposition();
		BallReposition();
	}

	public override void _Process(float delta)
	{
		// UI Button managing
		if (Input.IsActionJustPressed("fullscreen_toggle"))
			OS.WindowFullscreen = !OS.WindowFullscreen;
		if (Input.IsActionJustPressed("debug_vsync_toggle"))
			OS.VsyncEnabled	= !OS.VsyncEnabled;
		if (Input.IsActionJustPressed("restart"))
			GetTree().ReloadCurrentScene();

		// UI Scores managing
		GetNode<Label>("ScoresContainer/P1Score").Text = Convert.ToString(GetNode<Paddle>("Player1").Score);
		GetNode<Label>("ScoresContainer/P2Score").Text = Convert.ToString(GetNode<Paddle>("Player2").Score);
	}
}
