using Godot;
using System;

public class Ball : KinematicBody2D
{
    [Export]
    private int initialSpeed;
    
    private int _speed;
    private Vector2 _direction;

	public Ball()
	{
        GD.Randomize();

        _direction.x = GD.RandRange(-1, 1) > 0 ? 1 : -1;
        _direction.y = (float)GD.RandRange(-.75, .75);
	}

	public int Speed { get => _speed; set => _speed = value; }
	public Vector2 Direction { get => _direction; set => _direction = value; }

    public override void _Ready()
    {
        Speed = initialSpeed;
    }

    public override void _PhysicsProcess(float delta)
    {
        // Move ball
        Vector2 velocity = new Vector2();
        Vector2 screenSize = GetViewport().Size;

        velocity.x = Speed * Direction.x * delta;
        velocity.y = Speed * Direction.y * delta;

        KinematicCollision2D collision = MoveAndCollide(velocity);

        // Outside scene detection
        float spriteHeight = GetNode<Sprite>("Sprite").Texture.GetHeight() - 26;

        if ((Position.y - spriteHeight / 2 < 0 && Direction.y < 0)
        || (Position.y + spriteHeight / 2 > screenSize.y && Direction.y > 0))
        {
            Direction = new Vector2(Direction.x, -Direction.y);
        }

        if (Position.x < 0 || Position.x > screenSize.x)
        {
			GetTree().ReloadCurrentScene();
        }
    }
}
