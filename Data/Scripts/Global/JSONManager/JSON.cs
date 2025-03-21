using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

public class JSON
{
    private string _pathDialogues = "res://Data/Dialogs/";
    private string _pathPams = "res://Data/PAMS/";
    private string _pathChoices = "user://Saves/";
    private Directory Directory = new Directory();
    private JsonSerializerSettings _settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

public JSON()
    {
        Global.Settings.Saves = GetSaves();
        Global.Settings.Saves.Sort();
    }

    public List<NPCPAMS> GetNpcpams()
    {
        FileAccess file = FileAccess.Open($"{_pathPams}{Global.Settings.GameSettings.CurrentLocation}.json", FileAccess.ModeFlags.Read);
        string json = file?.GetAsText() ?? "";
        file?.Close();
        return JsonConvert.DeserializeObject<List<NPCPAMS>>(json);
    }

    public List<NPCDialogue> GetDialogues()
    {
        FileAccess file = FileAccess.Open($"{_pathDialogues}{Global.Settings.GameSettings.CurrentLocation}.json", FileAccess.ModeFlags.Read);
        string json = file?.GetAsText() ?? "";
        file?.Close();
        return JsonConvert.DeserializeObject<List<NPCDialogue>>(json);
    }

    public List<PlayerChoice> GetPlayerChoices()
    {
        FileAccess file = FileAccess.Open($"{_pathChoices}{Global.Settings.CurrentSave}/Choices/{Global.Settings.GameSettings.CurrentLocation}.json", FileAccess.ModeFlags.Read);
        string json = file.GetAsText();
        file.Close();
        return JsonConvert.DeserializeObject<List<PlayerChoice>>(json);
    }

    public void SetPlayerChoices(List<PlayerChoice> playerChoices)
    {
        FileAccess file = FileAccess.Open($"{_pathChoices}{Global.Settings.CurrentSave}/Choices/{Global.Settings.GameSettings.CurrentLocation}.json", FileAccess.ModeFlags.Write);
        string json = JsonConvert.SerializeObject(playerChoices, Formatting.Indented);
        file.StoreString(json);
        file.Close();
    }

    public void SaveGame()
    {
        FileAccess gameFile = FileAccess.Open($"{_pathChoices}{Global.Settings.CurrentSave}/GameSettings.json", FileAccess.ModeFlags.Write);
        FileAccess playerFile = FileAccess.Open($"{_pathChoices}{Global.Settings.CurrentSave}/PlayerSettings.json", FileAccess.ModeFlags.Write);
        Global.Settings.PlayerSettings.Position = Global.SceneObjects.Player?.Position ?? new Vector2(-3100, 30);
        Global.Settings.PlayerSettings.Health = Global.SceneObjects?.Player?.Health ?? 300;
        Global.Settings.PlayerSettings.Items = Global.SceneObjects.Player?.Inventory?.Items ?? Global.Settings.PlayerSettings.Items;
        Global.Settings.PlayerSettings.Armor = Global.SceneObjects.Player?.Inventory?.Armor ?? Global.Settings.PlayerSettings.Armor;
        Global.Settings.PlayerSettings.ActiveItems = Global.SceneObjects.Player?.Inventory?.ActiveItems ?? Global.Settings.PlayerSettings.ActiveItems;
        string gameJson = JsonConvert.SerializeObject(Global.Settings.GameSettings, Formatting.Indented);
        string playerJson = JsonConvert.SerializeObject(Global.Settings.PlayerSettings, Formatting.Indented, _settings);
        gameFile.StoreString(gameJson);
        playerFile.StoreString(playerJson);
        gameFile.Close();
        playerFile.Close();
    }

    public void LoadGame(string save)
    {
        Global.Settings.CurrentSave = save;
        FileAccess gameFile = FileAccess.Open($"{_pathChoices}{save}/GameSettings.json", FileAccess.ModeFlags.Read);
        FileAccess playerFile = FileAccess.Open($"{_pathChoices}{save}/PlayerSettings.json", FileAccess.ModeFlags.Read);
        string gameJson = gameFile.GetAsText();
        string playerJson = playerFile.GetAsText();
        gameFile.Close();
        playerFile.Close();
        JObject JPlayer = JObject.Parse(playerJson);
        Global.Settings.GameSettings = JsonConvert.DeserializeObject<GameSettings>(gameJson);
        Global.Settings.PlayerSettings = JsonConvert.DeserializeObject<PlayerSettings>(playerJson, _settings);
        Global.CutSceneData.LoadCutSceneData();
    }

    public void NewGame(string saveName, int saveNumber)
    {
        Directory.CreateSave(saveName, saveNumber);
        Global.SceneObjects.Player = null;
        SaveGame();
        Global.CutSceneData.LoadCutSceneData();
    }

    public int GetSaveNumber(string save)
    {
        FileAccess file = FileAccess.Open($"{_pathChoices}{save}/GameSettings.json", FileAccess.ModeFlags.Read);
        string json = file.GetAsText();
        file.Close();
        var gameSettings = JsonConvert.DeserializeObject<GameSettings>(json);
        return gameSettings.SaveNumber;
    }

    public List<Save> GetSaves()
    {
        List<string> saves = Directory.GetSaveNames();
        List<Save> result = new List<Save>();
        for (int i = 0; i < saves.Count; i++)
        {
            Save save = new Save();
            save.Name = saves[i];
            save.Number = GetSaveNumber(saves[i]);
            result.Add(save);
        }
        return result;
    }

    public void DeleteGame(string saveName) =>
        Directory.DeleteSave(saveName);
}
