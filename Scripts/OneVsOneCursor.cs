using Godot;
using System;

enum Choice
{
    Yes, No
}

public class OneVsOneCursor : Node2D
{
    Choice choice;

    Vector2 prevPos;
    int time;
    int duration;

    Sprite sprite;
    Vector2 spriteSize;

    private void AnimateTween(Vector2 startPos, Vector2 changePos)
    {
        if (time < duration) time++;
        var x = EasingFunction.EaseOutQuint(time, startPos.x, changePos.x - startPos.x, duration);
        var y = EasingFunction.EaseOutQuint(time, startPos.y, changePos.y - startPos.y, duration);
        Position = new Vector2(x, y);
    }

    private void ResetTween()
    {
        prevPos = Position;
        time    = 0;
    }

    public override void _Ready()
    {
        choice = Choice.No;

        prevPos = Position;
        time = 0;
        duration = 45;

        sprite      = GetNode<Sprite>("Sprite");
        spriteSize  = sprite.Texture.GetSize() * sprite.Scale;

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
            if (choice == Choice.Yes)
                choice = Choice.No;
            else
                choice = Choice.Yes;

            ResetTween();
        }

        if (Input.IsActionJustPressed("ui_accept"))
        {
            GetTree().Paused = false;

            if (choice == Choice.Yes)
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
            case Choice.Yes:
                label = marginContainer.GetNode<Label>("HBoxContainer/ChoiceContainer/Yes");
                break;
            case Choice.No:
                label = marginContainer.GetNode<Label>("HBoxContainer/ChoiceContainer/No");
                break;
        }

        float x         = menuContainer.RectPosition.y + 112;
        float y         = menuContainer.RectPosition.y + marginContainer.RectPosition.y + label.RectPosition.y + label.RectSize.y / 2 ;
        var labelPos    = new Vector2(x, y);

        GD.Print(labelPos);

        AnimateTween(prevPos, labelPos);
    }
}
