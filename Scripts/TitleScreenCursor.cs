using Godot;
using System;

enum Menu
{
    Title, Option
}

enum MenuIndex
{
    Play, Options, Quit
}

enum OptionMenuIndex
{
    Resolution, Fullscreen, Vsync, Input
}

public class TitleScreenCursor : Node2D
{
    Menu menu;
    MenuIndex menuIndex;
    OptionMenuIndex optionMenuIndex;

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

    public override void _Ready()
    {
        menu            = Menu.Title;
        menuIndex       = MenuIndex.Play;
        optionMenuIndex = OptionMenuIndex.Resolution;

        prevPos     = Position;
        time        = 0;
        duration    = 45;

        sprite      = GetNode<Sprite>("Sprite");
        spriteSize  = sprite.Texture.GetSize() * sprite.Scale;
    }

    public override void _Process(float delta)
    {
        NodePath titleScreenGuiPath = "/root/TitleScreen/TitleScreenGUI";
        NodePath optionScreenGuiPath = "/root/TitleScreen/OptionScreenGUI";
        var titleScreenGUI = GetNode<Control>(titleScreenGuiPath);
        var optionScreenGUI = GetNode<Control>(optionScreenGuiPath);

        if (menu == Menu.Title)
        {
            NodePath titleScreenMenuPath = titleScreenGuiPath + "/VBoxContainer/MarginContainer/TitleMenu";
            var label = GetNode<Label>(titleScreenMenuPath + "/PlayLabel");
            var vBoxContainer = titleScreenGUI.GetNode<VBoxContainer>("VBoxContainer");
            var marginContainer = titleScreenGUI.GetNode<MarginContainer>("VBoxContainer/MarginContainer");

            if (Input.IsActionJustPressed("ui_left"))
            {
                if (menuIndex == MenuIndex.Play)
                    menuIndex = MenuIndex.Quit;
                else
                    menuIndex--;

                prevPos = Position;
                time = 0;
            }

            if (Input.IsActionJustPressed("ui_right"))
            {
                if (menuIndex == MenuIndex.Quit)
                    menuIndex = MenuIndex.Play;
                else
                    menuIndex++;

                prevPos = Position;
                time = 0;
            }

            if (Input.IsActionJustPressed("ui_accept"))
            {
                switch (menuIndex)
                {
                    case MenuIndex.Play:
                        GetTree().ChangeScene("res://Scenes/Rooms/Room1v1.tscn");
                        break;
                    case MenuIndex.Options:
                        prevPos = Position;
                        time = 0;

                        menu = Menu.Option;
                        optionScreenGUI.Visible = true;
                        titleScreenGUI.Visible = false;
                        break;
                    case MenuIndex.Quit:
                        GetTree().Quit();
                        break;
                }
            }

            switch (menuIndex)
            {
                case MenuIndex.Play:
                    label = GetNode<Label>(titleScreenMenuPath + "/PlayLabel");
                    break;
                case MenuIndex.Options:
                    label = GetNode<Label>(titleScreenMenuPath + "/OptionsLabel");
                    break;
                case MenuIndex.Quit:
                    label = GetNode<Label>(titleScreenMenuPath + "/QuitLabel");
                    break;
            }
            
            float x = vBoxContainer.RectPosition.x + label.MarginRight - label.RectSize.x / 2;
            float y = vBoxContainer.RectPosition.y + marginContainer.RectPosition.y + label.RectSize.y / 2;
            var labelPos = new Vector2(x, y);

            AnimateTween(prevPos, labelPos);
        }
        else
        {
            NodePath optionScreenMenuPath = optionScreenGuiPath + "/MarginContainer/VBoxContainer/OptionMenu";
            var label = GetNode<Label>(optionScreenMenuPath + "/ResolutionValue");
            var marginContainer = optionScreenGUI.GetNode<MarginContainer>("MarginContainer");
            var gridContainer = optionScreenGUI.GetNode<GridContainer>("MarginContainer/VBoxContainer/OptionMenu");

            if (Input.IsActionJustPressed("ui_cancel"))
            {
                prevPos = Position;
                time = 0;

                menu = Menu.Title;
                titleScreenGUI.Visible = true;
                optionScreenGUI.Visible = false;
            }
            
            if (Input.IsActionJustPressed("ui_up"))
            {
                if (optionMenuIndex == OptionMenuIndex.Resolution)
                    optionMenuIndex = OptionMenuIndex.Input;
                else
                    optionMenuIndex--;

                prevPos = Position;
                time = 0;
            }

            if (Input.IsActionJustPressed("ui_down"))
            {
                if (optionMenuIndex == OptionMenuIndex.Input)
                    optionMenuIndex = OptionMenuIndex.Resolution;
                else
                    optionMenuIndex++;

                prevPos = Position;
                time = 0;
            }

            switch (optionMenuIndex)
            {
                case OptionMenuIndex.Resolution:
                    label = GetNode<Label>(optionScreenMenuPath + "/ResolutionValue");
                    break;
                case OptionMenuIndex.Fullscreen:
                    label = GetNode<Label>(optionScreenMenuPath + "/FullscreenValue");
                    break;
                case OptionMenuIndex.Vsync:
                    label = GetNode<Label>(optionScreenMenuPath + "/VsyncValue");
                    break;
                case OptionMenuIndex.Input:
                    label = GetNode<Label>(optionScreenMenuPath + "/InputValue");
                    break;
            }
            
            float x = marginContainer.MarginLeft + label.MarginRight - spriteSize.x / 2 + 16;
            float y = marginContainer.RectPosition.y + gridContainer.RectPosition.y + label.RectPosition.y + label.RectSize.y / 2;
            var labelPos = new Vector2(x, y);

            AnimateTween(prevPos, labelPos);
        }
    }
}
