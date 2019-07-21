using Godot;
using System;

enum Role
{
	P1, P2, IA
}

public class Paddle : KinematicBody2D
{
	[Export]
	private int _sensitivity;
	[Export]
	private int _speed;
	[Export]
	private Role _role;

	private Color _color;
	private int _score;

	private string keyLeft;
	private string keyRight;
	private string keyUp;
	private string keyDown;

	public Paddle()
	{
		GD.Randomize();
		_color = new Color((int)GD.RandRange(0, Math.Pow(2, 31)));
		_color.a = 1;
		_score = 0;
	}

	public int Sensitivity { get => _sensitivity; set => _sensitivity = value; }
	public int Speed { get => _speed; set => _speed = value; }
	public Color Color { get => _color; set => _color = value; }
	internal Role Role { get => _role; set => _role = value; }
	public int Score { get => _score; set => _score = value; }

	public void AttributeKeys()
	{
		switch (Role)
		{
			case Role.P1:
				keyUp = "P1_up";
				keyDown = "P1_down";
				break;
			case Role.P2:
				keyUp = "P2_up";
				keyDown = "P2_down";
				break;
		}
	}

	public override void _Ready()
	{
		GetNode<Sprite>("Sprite").SelfModulate = Color;

		if (Role != Role.IA)
		{
			AttributeKeys();
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		// Not IA section
		if (Role != Role.IA)
		{
			// Move paddle
			float direction = Input.GetActionStrength(keyDown) - Input.GetActionStrength(keyUp);
			float positionLimit;

			Vector2 velocity;

			velocity.x = 0;
			velocity.y = ((Speed * Sensitivity) * direction) * delta;

			int spriteHeight = GetNode<Sprite>("Sprite").Texture.GetHeight() - 32;
			int yPosMin = 0 + spriteHeight / 2, yPosMax = (int)OS.GetScreenSize().y - spriteHeight / 2;

			KinematicCollision2D collision = MoveAndCollide(velocity);

			positionLimit = Mathf.Clamp(Position.y, yPosMin, yPosMax);
			Position = new Vector2(Position.x, positionLimit);

			// Flip ball when colliding
			if (collision != null)
			{
				Ball ball = (Ball)collision.Collider;
				float yDir = (ball.Position.y - Position.y) / 100;
				
				if (Position.x < ball.Position.x)
					ball.Direction = new Vector2(1, yDir);
				else
					ball.Direction = new Vector2(-1, yDir);
			}
		}
	}
}
