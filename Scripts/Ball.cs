using Godot;
using System;

public class Ball : KinematicBody2D
{
	private int initialSpeed;

	private int _speed;
	private Vector2 _direction;

	public Ball()
	{
		GD.Randomize();

		_direction.x = GD.RandRange(-1, 1) > 0 ? 1 : -1;
		_direction.y = (float)GD.RandRange(-.75, .75);
		initialSpeed = 300;
	}

	public int Speed { get => _speed; set => _speed = value; }
	public Vector2 Direction { get => _direction; set => _direction = value; }

	public void ResetBall()
	{
		Direction = new Vector2(-Direction.x, (float)GD.RandRange(-.75, .75));
		Position = OS.GetScreenSize() / 2;
		Speed = 0;

		GetParent().GetNode<Timer>("BallTimer").Start();
	}

	public void _onBallTimerTimeout()
	{
		Speed = initialSpeed;
	}

	public override void _PhysicsProcess(float delta)
	{
		// Move ball
		Vector2 velocity = new Vector2();

		velocity.x = Speed * Direction.x * delta;
		velocity.y = Speed * Direction.y * delta;

		KinematicCollision2D collision = MoveAndCollide(velocity);

		// Outside scene detection
		float spriteHeight = GetNode<Sprite>("Sprite").Texture.GetHeight() - 26;
		Vector2 screenSize = OS.GetScreenSize();

		if ((Position.y - spriteHeight / 2 < 0 && Direction.y < 0)
		|| (Position.y + spriteHeight / 2 > screenSize.y && Direction.y > 0))
		{
			Direction = new Vector2(Direction.x, -Direction.y);
		}

		if (Position.x < 0)
		{
			GetParent().GetNode<Paddle>("Player2").Score++;
			ResetBall();
		}

		if (Position.x > screenSize.x)
		{
			GetParent().GetNode<Paddle>("Player1").Score++;
			ResetBall();
		}
	}
}
