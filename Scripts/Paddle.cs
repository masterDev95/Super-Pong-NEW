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
	private int _spriteHeight;

	private string keyUp;
	private string keyDown;

	private float randY;

	public Paddle()
	{
		GD.Randomize();
		_color = new Color((int)GD.RandRange(0, Math.Pow(2, 31)));
		_color.a = 1;
		_score = 0;
	}

	public int Sensitivity { get => _sensitivity; set => _sensitivity = value; }
	public int Speed { get => _speed; set => _speed = value; }
	internal Role Role { get => _role; set => _role = value; }
	public Color Color { get => _color; set => _color = value; }
	public int Score { get => _score; set => _score = value; }
	public int SpriteHeight { get => _spriteHeight; set => _spriteHeight = value; }

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
		SpriteHeight = GetNode<Sprite>("Sprite").Texture.GetHeight();

		randY = (float)GD.RandRange(-(SpriteHeight / 2), SpriteHeight / 2);

		GetNode<Sprite>("Sprite").SelfModulate = Color;

		if (Role != Role.IA)
		{
			AttributeKeys();
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		// Move paddle
		int spriteHeight = SpriteHeight - 32;
		int yPosMin = 0 + spriteHeight / 2, yPosMax = (int)OS.GetScreenSize().y - spriteHeight / 2;

		float direction, positionLimit;

		KinematicBody2D Ball = GetParent().GetNode<KinematicBody2D>("Ball");
		
		Vector2 velocity = new Vector2(0, 0);

		if (Role != Role.IA)
		{
			direction = Input.GetActionStrength(keyDown) - Input.GetActionStrength(keyUp);
			velocity.y = ((Speed * Sensitivity) * direction) * delta;
		}
		else
		{
			direction = Ball.Position.y + randY - Position.y;

			if (GetName() == "Player1" && Ball.Position.x < OS.GetScreenSize().x / 2
			|| GetName() == "Player2" && Ball.Position.x > OS.GetScreenSize().x / 2)
			{
				velocity.y = Speed * direction * delta;
			}
		}

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

			randY = (float)GD.RandRange(-(SpriteHeight / 2), SpriteHeight / 2);
		}
	}
}
