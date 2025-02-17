using Godot;
using System;

public partial class SaveButton : TextureButton
{
    [Signal] public delegate void CurrentSaveNameEventHandler(string name);

    private Label _label;

    public string Text { get => _label.Text; set => _label.Text = value; }

    public SaveButton()
    {
        _label = new Label();
        AddChild(_label);
        _label.LabelSettings = new LabelSettings();
        _label.LabelSettings.FontSize = 48;
        _label.LabelSettings.Font = GD.Load<Font>("res://ComicoroRu_0.ttf");
        StretchMode = StretchModeEnum.KeepAspectCentered;
        _label.SetAnchorsPreset(LayoutPreset.Center);
        _label.AnchorTop = 0.2f;
        _label.AnchorBottom = 0.8f;
        _label.AnchorLeft = 0.5f;
        _label.AnchorRight = 0.5f;
        CustomMinimumSize = new Vector2(0, 100);
        TextureNormal = GD.Load<Texture2D>("res://Data/Textures/Menu/Buttons/LongButton/NormalLongButton.png");
        TextureHover = GD.Load<Texture2D>("res://Data/Textures/Menu/Buttons/LongButton/HoverLongButton.png");
        TexturePressed = GD.Load<Texture2D>("res://Data/Textures/Menu/Buttons/LongButton/PressedLongButton.png");
        TextureFocused = GD.Load<Texture2D>("res://Data/Textures/Menu/Buttons/LongButton/HoverLongButton.png");
    }

    public override void _Ready()
    {
        Pressed += OnPressed;
    }

    private void OnPressed()
    {
        EmitSignal(SignalName.CurrentSaveName, Name);
    }
}
