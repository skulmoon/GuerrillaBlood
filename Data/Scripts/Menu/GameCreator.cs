using Godot;
using System;
using System.Collections.Generic;

public partial class GameCreator : Control
{
    private int _currentButton;
    private TextEdit _textEdit;
    private VBoxContainer _container;
    private TextureButton _buttonNew;
    private TextureButton _buttonExit;

    [Export] public Label Label { get; set; }

    public override void _Ready()
    {
        _buttonExit = GetNode<TextureButton>("ButtonExit");
        _buttonExit.Pressed += ExitButtonPressed;
        _buttonNew = GetNode<TextureButton>("ButtonNew");
        _buttonNew.Pressed += NewButtonPressed;
        _textEdit = GetNode<TextEdit>("TextEdit");
    }

    public void NewButtonPressed()
    {
        _textEdit.Text = _textEdit.Text.Trim();
        if (Global.Settings.Saves.Find(x => x.Name == _textEdit.Text) != null)
        {
            Label.Text = "Сохранение с данным именем уже существует.";
        }
        else if (_textEdit.Text == string.Empty)
        {
            Label.Text = "Поле не должно быть пустым или состоять из пробелов.";
        }
        else
        {
            Global.JSON.NewGame(_textEdit.Text, Global.Settings.Saves.Count + 1);
            Global.Settings.IsNewButtonPressed = true;
            GetTree().ChangeSceneToFile($"res://Data/Scenes/Locations/{Global.Settings.GameSettings.CurrentLocation}.tscn");
        }
    }

    public void ExitButtonPressed() =>
        GetTree().ChangeSceneToFile("res://Data/Scenes/Menu/MainMenu.tscn");
}
