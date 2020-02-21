using Godot;
using System;

enum OptionMenuIndex
{
    Resolution, Fullscreen, Vsync, Input
}

public class OptionMenu : GridContainer
{
    private OptionMenuIndex _index;

    private Node2D _cursor;
    private Sprite _cursorSprite;
    private int _cursorTime;
    private float _cursorPrevX;
    private float _cursorPrevY;

    internal OptionMenuIndex Index { get => _index; set => _index = value; }

    private void AnimateCursor(float x1, float y1, float x2, float y2, int duration, ref int time)
    {
        if (time < duration) time++;
        var x = EasingFunction.EaseOutQuint(time, x1, x2 - x1, duration);
        var y = EasingFunction.EaseOutQuint(time, y1, y2 - y1, duration);
        _cursor.Position = new Vector2(x, y);
    }

    public override void _Ready()
    {
        Index = OptionMenuIndex.Resolution;
        _cursor = GetNode<Node2D>("/root/TitleScreen/OptionScreenGUI/Cursor");
        _cursorSprite = _cursor.GetNode<Sprite>("Sprite");
        _cursorTime = 0;
        _cursorPrevX = _cursor.Position.x;
        _cursorPrevY = _cursor.Position.y;
        SetProcess(false);
    }

    public override void _Process(float delta)
    {
        NodePath optionScreenPath = "/root/TitleScreen/OptionScreenGUI";
        NodePath titleScreenPath = "/root/TitleScreen/TitleScreenGUI";
        var optionScreen = GetNode<Control>(optionScreenPath);
        var titleScreen = GetNode<Control>(titleScreenPath);
        var marginCont = GetNode<MarginContainer>(optionScreenPath + "/MarginContainer");
        var label = GetNode<Label>("ResolutionValue");
        var labelX = marginCont.RectPosition.x + label.MarginRight - (float)((_cursorSprite.Texture.GetSize().x * _cursorSprite.Scale.x) / 2) + 16;
        float labelY;

        if (Input.IsActionJustPressed("ui_cancel"))
        {            
            titleScreen.Visible = true;
            optionScreen.Visible = false;

            titleScreen.GetNode("VBoxContainer/MarginContainer/TitleMenu").SetProcess(true);
            SetProcess(false);
        }

        if (Input.IsActionJustPressed("ui_up"))
        {
            if (Index == OptionMenuIndex.Resolution)
                Index = OptionMenuIndex.Input;
            else
                Index--;

            _cursorPrevX = _cursor.Position.x;
            _cursorPrevY = _cursor.Position.y;
            _cursorTime = 0;
        }

        if (Input.IsActionJustPressed("ui_down"))
        {
            if (Index == OptionMenuIndex.Input)
                Index = OptionMenuIndex.Resolution;
            else
                Index++;

            _cursorPrevX = _cursor.Position.x;
            _cursorPrevY = _cursor.Position.y;
            _cursorTime = 0;
        }

        switch (Index)
        {
            case OptionMenuIndex.Resolution:
                label = GetNode<Label>("ResolutionValue");
                break;
            case OptionMenuIndex.Fullscreen:
                label = GetNode<Label>("FullscreenValue");
                break;
            case OptionMenuIndex.Vsync:
                label = GetNode<Label>("VsyncValue");
                break;
            case OptionMenuIndex.Input:
                label = GetNode<Label>("InputValue");
                break;
        }

        labelY = RectPosition.y + marginCont.RectPosition.y + label.MarginBottom - (label.MarginBottom - label.RectPosition.y) / 2;
        AnimateCursor(_cursorPrevX, _cursorPrevY, labelX, labelY, 45, ref _cursorTime);
    }
}
