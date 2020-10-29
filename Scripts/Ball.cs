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

	public int Speed
	{
		get => _speed;
		set => _speed = value;
	}

	public Vector2 Direction
	{
		get => _direction;
		set => _direction = value;
	}

	public void ResetBall()
	{
		_direction = new Vector2(-_direction.x, (float)GD.RandRange(-.75, .75));
		Position = GetViewport().GetVisibleRect().Size / 2;
		_speed = 0;

		GetParent().GetNode<Timer>("BallTimer").Start();
	}

	public void _onBallTimerTimeout()
	{
		_speed = initialSpeed;
	}

	public override void _PhysicsProcess(float delta)
	{
		// Move ball
		Vector2 velocity = new Vector2();

		velocity.x = _speed * _direction.x * delta;
		velocity.y = _speed * _direction.y * delta;

		KinematicCollision2D collision = MoveAndCollide(velocity);

		// Outside scene detection
		var spriteHeight = GetNode<Sprite>("Sprite").Texture.GetHeight() - 26;
		var screenSize = GetViewport().GetVisibleRect().Size;

		if ((Position.y - spriteHeight / 2 < 0 && _direction.y < 0)
		|| (Position.y + spriteHeight / 2 > screenSize.y && _direction.y > 0))
		{
			_direction = new Vector2(_direction.x, -_direction.y);
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
