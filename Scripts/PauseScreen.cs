using Godot;
using System;

enum PauseIndex
{
    Yes, No
}

public class PauseScreen : Node2D
{
    private PauseIndex _index;

	public PauseScreen()
	{
        _index = PauseIndex.No;
	}

    internal PauseIndex Index
    {
        get => _index;
        set => _index = value;
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
                if (Index == PauseIndex.Yes)
                    Index++;
                else
                    Index--;
            }

            if (Input.IsActionJustPressed("ui_accept"))
            {
                GetTree().Paused = false;            
                
                if (Index == PauseIndex.Yes)
                    GetTree().ChangeScene("res://Scenes/Rooms/TitleScreen.tscn");
                else
                    Visible = false;
            }

            switch (Index)
            {
                case PauseIndex.Yes:
                    cursor.SetPosition(yesLabel.RectPosition);
                    break;
                case PauseIndex.No:
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
}
