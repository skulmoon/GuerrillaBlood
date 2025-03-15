using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;

public partial class Player : CharacterBody2D
{
    private PlayerInteractionArea _interactionArea;
    private PlayerInventory _inventory;
    private SpreadLines _lines = new SpreadLines();
    private float _health = 300;

    public PlayerInventory Inventory { get => _inventory; private set => _inventory = value; }
    public IActiveItem ActiveItem { get; set; }
    [Export] public Sprite2D Sprite { get; set; }
    [Export] public Vector2 TargetPosition { get; set; }
    [Export] public float Speed { get; set; } = 6000;
    [Export] public float Acceleration { get; set; } = 2;
    public float CurrentSpeedMultiplier { get; set; } = 1;
    public virtual float Health
    {
        get => _health;
        set
        {
            _health = value;
            Global.SceneObjects.Hp.Value = _health;
            if (value <= 0)
            {
                Global.SceneObjects.CurrentMission = null;
                Global.SceneObjects.DeathMenu.Open();
            }
        }
    }

    public override void _Ready()
    {
        AddChild(_lines);
        _interactionArea = GetNode<PlayerInteractionArea>("PlayerInteractionArea");
        Inventory = new PlayerInventory();
        Global.SceneObjects.TakePlayerSettings(this);
        Global.SceneObjects.Hp.Value = _health;
        Global.SceneObjects.Hp.Visible = true;
        Global.SceneObjects.Player = this;
        _lines.Hide();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!Global.Settings.CutScene)
        {
            Move(delta);
            if (ActiveItem is Weapon2D weapon)
            {
                weapon.CurrentSpread += 1 / weapon.Stabilization * weapon.Spread * (float)delta * 50 * Vector2.FromAngle(Sprite.Rotation).DistanceTo(GlobalPosition.DirectionTo(GetGlobalMousePosition()));
                _lines.SetLinesRotation(Sprite.Rotation, weapon.CurrentSpread, delta);
                if (Input.IsActionJustPressed("reload") && weapon.Reload.TimeLeft == 0)
                {
                    Global.Sound.PlaySound(weapon.ReloadSound, 0.1f);
                    weapon.Reload.Start();
                }
            }
            if (Input.IsActionPressed("first_action") && !Input.IsActionPressed("acceleration"))
                ActiveItem?.FirstAction(Sprite.Rotation, this);
            if (Input.IsActionJustPressed("second_action"))
                ActiveItem?.SecondAction();
            if (Input.IsActionJustPressed("first_active_cell"))
                ChangeActiveItem(1);
            else if (Input.IsActionJustPressed("second_active_cell"))
                ChangeActiveItem(2);
            else if (Input.IsActionJustPressed("third_active_cell"))
                ChangeActiveItem(3);
        }
    }

    private void Move(double delta)
    {
        Vector2 direction = new Vector2(Input.GetAxis("left", "right"), Input.GetAxis("up", "down")).Normalized();
        if (Input.IsActionPressed("acceleration") && !(ActiveItem?.ActiveSecondAction ?? false))
        {
            delta *= Acceleration;
            Sprite.Rotation = Vector2.FromAngle(Sprite.Rotation).Lerp(direction, 13 * (float)delta).Angle();
            if (ActiveItem != null)
                _lines.Hide();
        }
        else
        {
            Sprite.Rotation = Vector2.FromAngle(Sprite.Rotation).Lerp(GlobalPosition.DirectionTo(GetGlobalMousePosition()), 10 * (float)delta).Angle();
            if (ActiveItem != null)
                _lines.Show();
        }
        Velocity = direction * (Speed * CurrentSpeedMultiplier * (float)delta);
        MoveAndSlide();
        _interactionArea.PayerDirection = direction;
        if (direction != Vector2.Zero && ActiveItem is Weapon2D weapon)
            weapon.CurrentSpread += (weapon.Spread / weapon.Stabilization) * (float)delta * 10;
    }  

    private void ChangeActiveItem(int ItemNumber)
    {
        CurrentSpeedMultiplier = 1;
        Global.SceneObjects.InventoryMenu.GetNode<InventoryItems>("Weapons").Cells[ItemNumber - 1].GrabFocus();
        if (ActiveItem != null)
            RemoveChild((Node)ActiveItem);
        if (Inventory.ActiveItems[ItemNumber - 1] != null)
        {
            if (((ActiveItem)Inventory.ActiveItems[ItemNumber - 1]).Object == null)
                ((ActiveItem)Inventory.ActiveItems[ItemNumber - 1]).UpdateItem();
            ActiveItem = ((ActiveItem)Inventory.ActiveItems[ItemNumber - 1]).Object;
            AddChild((Node)ActiveItem);
            if (ActiveItem is Weapon2D weapon)
            {
                _lines.Show();
                CurrentSpeedMultiplier = weapon.SpeedMultiplier;
                Global.SceneObjects.Ammunition.Visible = true;
                Global.SceneObjects.Ammunition.MaxValue = weapon.Ammunition;
                Global.SceneObjects.Ammunition.Value = weapon.CurrentAmmunition;
            }
            else
                Global.SceneObjects.Ammunition.Visible = false;
        }
        else
        {
            Global.SceneObjects.Ammunition.Visible = false;
            ActiveItem = null;
            _lines.Hide();
        }
    }

    public void TakeDamage(float damage) =>
        Health -= damage;
}
