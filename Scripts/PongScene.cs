using Godot;
using System;

public class PongScene : Node2D
{
	public override void _Ready()
	{
		KinematicBody2D ball = GetNode<KinematicBody2D>("Ball");
		KinematicBody2D p1 = GetNode<KinematicBody2D>("Player1");
		KinematicBody2D p2 = GetNode<KinematicBody2D>("Player2");
		int xPlayersMargin = 64;
		float yPos = GetViewport().GetVisibleRect().Size.y / 2;
		float p2XPos = GetViewport().GetVisibleRect().Size.x - xPlayersMargin;

		ball.Position = GetViewport().GetVisibleRect().Size / 2;
		p1.Position = new Vector2(xPlayersMargin, yPos);
		p2.Position = new Vector2(p2XPos, yPos);
	}

	public override void _Process(float delta)
	{
		// UI Scores managing
		GetNode<Label>("ScoresContainer/P1Score").Text = Convert.ToString(GetNode<Paddle>("Player1").Score);
		GetNode<Label>("ScoresContainer/P2Score").Text = Convert.ToString(GetNode<Paddle>("Player2").Score);
	}
}
