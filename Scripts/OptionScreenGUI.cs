using Godot;
using System;

public class OptionScreenGUI : Control
{
    public override void _Process(float delta)
    {
        var resolutionLabel = GetNode<Label>("MarginContainer/VBoxContainer/OptionMenu/ResolutionValue");
        var fullscreenLabel = GetNode<Label>("MarginContainer/VBoxContainer/OptionMenu/FullscreenValue");
        var vSyncLabel      = GetNode<Label>("MarginContainer/VBoxContainer/OptionMenu/VsyncValue");

        resolutionLabel.Text = $"{OS.WindowSize.x}x{OS.WindowSize.y}";
        fullscreenLabel.Text = OS.WindowFullscreen ? "Oui" : "Non";
        vSyncLabel.Text      = OS.VsyncEnabled ? "Oui" : "Non";
    }
}
