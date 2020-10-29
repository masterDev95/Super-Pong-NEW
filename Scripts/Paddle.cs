using Godot;
using System;

public class Paddle : KinematicBody2D
{
	[Export]
	private int _sensitivity;
	[Export]
	private int _speed;
	[Export]
	private Roles _role;

	private Color _color;
	private string keyUp;
	private string keyDown;
	private int _spriteWidth;
	private int _spriteHeight;
	private float randY;

	public Paddle()
	{
		GD.Randomize();
		_color = new Color((int)GD.RandRange(0, Math.Pow(2, 31)));
		_color.a = 1;
		Score = 0;
	}

	public int Sensitivity { get => _sensitivity; set => _sensitivity = value; }
	public int Speed { get => _speed; set => _speed = value; }
	public int Score { get; set; }

	public override void _Ready()
	{
		_spriteWidth = GetNode<Sprite>("Sprite").Texture.GetWidth();
		_spriteHeight = GetNode<Sprite>("Sprite").Texture.GetHeight();

		randY = (float)GD.RandRange(-(_spriteHeight / 2), _spriteHeight / 2);

		GetNode<Sprite>("Sprite").SelfModulate = _color;

		if (_role != Roles.IA)
		{
			switch (_role)
			{
				case Roles.P1:
					keyUp = "P1_up";
					keyDown = "P1_down";
					break;
				case Roles.P2:
					keyUp = "P2_up";
					keyDown = "P2_down";
					break;
			}
		}
	}

	public void _onPaddleTimerTimeout()
	{
		randY = (float)GD.RandRange(-(_spriteHeight / 2) + 4, _spriteHeight / 2) + 4;
	}

	public override void _PhysicsProcess(float delta)
	{
		// Move paddle
		var paddleHeight = _spriteHeight - 32;
		var yPosMin = 0 + paddleHeight / 2;
		var yPosMax = (int)GetViewport().GetVisibleRect().Size.y - paddleHeight / 2;
		float direction, positionLimit;

		KinematicBody2D Ball = GetParent().GetNode<KinematicBody2D>("Ball");
		
		Vector2 velocity = new Vector2(0, 0);

		if (_role != Roles.IA)
		{
			direction = Input.GetActionStrength(keyDown) - Input.GetActionStrength(keyUp);
			velocity.y = ((Speed * Sensitivity) * direction) * delta;
		}
		else
		{
			direction = Ball.Position.y + randY - Position.y;

			if (Name == "Player1" && Ball.Position.x < GetViewport().GetVisibleRect().Size.x / 2
			|| Name == "Player2" && Ball.Position.x > GetViewport().GetVisibleRect().Size.x / 2)
			{
				velocity.y = Speed * direction * delta;
			}
		}

		KinematicCollision2D collision = MoveAndCollide(velocity);
		
		positionLimit = Mathf.Clamp(Position.y, yPosMin, yPosMax);
		Position = new Vector2(Position.x, positionLimit);

		// Collision
		if (collision != null)
		{
			Ball ball = (Ball)collision.Collider;
			float yDir = (ball.Position.y - Position.y) / 100;

			if (Position.x < ball.Position.x)
			{
				ball.Direction = new Vector2(1, yDir);
				ball.Position = new Vector2(Position.x + _spriteWidth / 2, ball.Position.y);
			}
			else
			{
				ball.Direction = new Vector2(-1, yDir);
				ball.Position = new Vector2(Position.x - _spriteWidth / 2, ball.Position.y);
			}
			
			ball.Speed += 50;
		}
	}

	private enum Roles
	{
		P1, P2, IA
	}
}
