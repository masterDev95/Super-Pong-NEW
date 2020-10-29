using Godot;
using System;

public class OneVsOneCursor : Node2D
{
    private Choices choice;
    private Vector2 prevPos;
    private int time;
    private int duration;

    public override void _Ready()
    {
        choice = Choices.No;

        prevPos = Position;
        time = 0;
        duration = 45;

        SetProcess(false);
    }

    public override void _Process(float delta)
    {
        var root            = GetParent().GetParent<Node2D>();
        var menuContainer   = root.GetNode<VBoxContainer>("Background/MenuContainer");
        var marginContainer = root.GetNode<MarginContainer>("Background/MenuContainer/MarginContainer");
        var label           = marginContainer.GetNode<Label>("HBoxContainer/ChoiceContainer/No");

        if (Input.IsActionJustPressed("ui_up") || Input.IsActionJustPressed("ui_down"))
        {
            if (choice == Choices.Yes)
                choice = Choices.No;
            else
                choice = Choices.Yes;

            prevPos = Position;
            time = 0;
        }

        if (Input.IsActionJustPressed("ui_accept"))
        {
            GetTree().Paused = false;

            if (choice == Choices.Yes)
                GetTree().ChangeScene("res://Scenes/Rooms/TitleScreen.tscn");
            else
                root.Visible = false;

            SetProcess(false);
        }

        if (Input.IsActionJustPressed("ui_cancel"))
        {
            root.Visible = false;
            SetProcess(false);
        }

        switch (choice)
        {
            case Choices.Yes:
                label = marginContainer.GetNode<Label>("HBoxContainer/ChoiceContainer/Yes");
                break;
            case Choices.No:
                label = marginContainer.GetNode<Label>("HBoxContainer/ChoiceContainer/No");
                break;
        }

        var x = menuContainer.RectPosition.y + 112;
        var y = menuContainer.RectPosition.y + marginContainer.RectPosition.y + label.RectPosition.y + label.RectSize.y / 2;
        var labelPos = new Vector2(x, y);

        // Ease movement

        if (time < duration) time++;

        x = EasingFunctions.EaseOutQuint(time, prevPos.x, labelPos.x - prevPos.x, duration);
        y = EasingFunctions.EaseOutQuint(time, prevPos.y, labelPos.y - prevPos.y, duration);

        Position = new Vector2(x, y);
    }
    private enum Choices
    {
        Yes, No
    }
}
