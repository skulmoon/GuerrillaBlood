using Godot;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;

public class SceneObjects
{
    private Player _player;
    private DialogPanel _panel;
    private Storage _storage;

    private Action<Node> _onStorageReady;
    private Action<Node> _onPlayerChanged;
    private Action<Node> _onDialoguePanelChanged;

    public Storage Storage { get => _storage; set => _storage = (Storage)SetObject(value, ref _onStorageReady); }
    public Player Player { get => _player; set => _player = (Player)SetObject(value, ref _onPlayerChanged); }
    public DialogPanel DialoguePanel { get => _panel; set => _panel = (DialogPanel)SetObject(value, ref _onDialoguePanelChanged); }
    public HpProgressBar Hp { get; set; }
    public AmunitionProgressBar Ammunition { get; set; }
    public DialogueTexture DialogueTexture { get; set; }
    public Mission CurrentMission { get; set; }
    public List<NPC> Npcs { get; set; } = new List<NPC>();
    public EnemyFabric EnemyFabric { get; set; }
    public List<Enemy> Enemies { get; set; } = new List<Enemy>();
    public InventoryMenu InventoryMenu { get; set; }
    public WinMenu WinMenu { get; set; }
    public DeathMenu DeathMenu { get; set; }

    public event Action<Node> OnEnemyDie = (x) => { };
    public event Action<Node> OnStorageReady { add => Subscribe(ref _onStorageReady, value, _player); remove => _onStorageReady -= value; }
    public event Action<Node> OnPlayerChanged { add => Subscribe(ref _onPlayerChanged, value, _player); remove => _onPlayerChanged -= value; }
    public event Action<Node> OnDialoguePanelChanged { add => Subscribe(ref _onDialoguePanelChanged, value, _player); remove => _onDialoguePanelChanged -= value; }

    public void TakePlayerSettings(Player player)
    {
        player.Position = Global.Settings.PlayerSettings.Position;
        player.Health = Global.Settings.PlayerSettings.Health;
        player.Inventory.Items = Global.Settings.PlayerSettings.Items;
        player.Inventory.ActiveItems = Global.Settings.PlayerSettings.ActiveItems;
    }

    private Node SetObject(Node value, ref Action<Node> handler)
    {
        handler?.Invoke(value);
        return value;
    }

    private void Subscribe(ref Action<Node> handler, Action<Node> subscriber, Node node)
    {
        handler += subscriber;
        if (node != null)
            subscriber.Invoke(node);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        Enemies.Remove(enemy);
        OnEnemyDie.Invoke(enemy);
    }
}
