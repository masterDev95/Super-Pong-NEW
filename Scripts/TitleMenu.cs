using Godot;
using System;

enum MenuIndex
{
    Play, Options, Quit
}

public class TitleMenu : HBoxContainer
{
    private MenuIndex _index;

    Label prevItem;
    Label item;

	internal MenuIndex Index { get => _index; set => _index = value; }

	public override void _Ready()
    {
        Index = MenuIndex.Play;

        prevItem = GetNode<Label>("PlayLabel");
        item = prevItem;
    }

    public override void _Process(float delta)
    {

        bool indexHasChanged = false;

        if (Input.IsActionJustPressed("ui_left"))
        {
            if (Index == MenuIndex.Play)
                Index = MenuIndex.Quit;
            else
                Index--;
            
            indexHasChanged = true;
        }

        if (Input.IsActionJustPressed("ui_right"))
        {
            if (Index == MenuIndex.Quit)
                Index = MenuIndex.Play;
            else
                Index++;

            indexHasChanged = true;
        }

        if (Input.IsActionJustPressed("ui_accept"))
        {
            switch (Index)
            {
                case MenuIndex.Play:
                    GetTree().ChangeScene("res://Scenes/Rooms/Room1v1.tscn");
                    break;
                case MenuIndex.Options:
                    var optionScreen = GetNode<Control>("/root/TitleScreen/OptionScreenGUI");
                    var titleScreen = GetNode<Control>("/root/TitleScreen/TitleScreenGUI");
                    
                    optionScreen.Visible = true;
                    titleScreen.Visible = false;

                    optionScreen.SetProcess(true);
                    SetProcess(false);

                    break;
                case MenuIndex.Quit:
                    GetTree().Quit();
                    break;
            }
        }
    
        if (indexHasChanged)
        {
            switch (Index)
            {
                case MenuIndex.Play:
                    item = GetNode<Label>("PlayLabel");
                    break;
                case MenuIndex.Options:
                    item = GetNode<Label>("OptionsLabel");
                    break;
                case MenuIndex.Quit:
                    item = GetNode<Label>("QuitLabel");
                    break;
            }

            prevItem.GetNode<NinePatchRect>("Background").Visible = false;
            item.GetNode<NinePatchRect>("Background").Visible = true;

            prevItem = item;
        }
    }
}
