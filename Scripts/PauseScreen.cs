using Godot;
using System;

public class PauseScreen : Node2D
{
    private PauseIndexes _index;

	public PauseScreen()
	{
        _index = PauseIndexes.No;
	}

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_cancel"))
            Visible = !Visible;

        if (Visible)
        {
            TextureRect cursor = GetNode<TextureRect>("Background/CenterContainer/TitleContainer/HBoxContainer/MarginContainer/HBoxContainer/Cursor");

            Vector2 cursorOffset = new Vector2(0, 3);

            Label yesLabel = GetNode<Label>("Background/CenterContainer/TitleContainer/HBoxContainer/MarginContainer/HBoxContainer/ChoiceContainer/Yes");
            Label noLabel = GetNode<Label>("Background/CenterContainer/TitleContainer/HBoxContainer/MarginContainer/HBoxContainer/ChoiceContainer/No");

            GetTree().Paused = true;

            if (Input.IsActionJustPressed("ui_up") || Input.IsActionJustPressed("ui_down"))
            {
                if (_index == PauseIndexes.Yes)
                    _index++;
                else
                    _index--;
            }

            if (Input.IsActionJustPressed("ui_accept"))
            {
                GetTree().Paused = false;
                
                if (_index == PauseIndexes.Yes)
                    GetTree().ChangeScene("res://Scenes/Rooms/TitleScreen.tscn");
                else
                    Visible = false;
            }

            switch (_index)
            {
                case PauseIndexes.Yes:
                    cursor.SetPosition(yesLabel.RectPosition);
                    break;
                case PauseIndexes.No:
                    cursor.SetPosition(noLabel.RectPosition);
                    break;
            }

            cursor.SetPosition(cursor.RectPosition + cursorOffset);
        }
        else
        {
            GetTree().Paused = false;
        }
    }

    private enum PauseIndexes
    {
        Yes, No
    }
}
