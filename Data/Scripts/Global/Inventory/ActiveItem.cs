using Godot;
using System;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json;

[GlobalClass]
public partial class ActiveItem : Item
{
    [JsonIgnore] private List<object> _parameters = new List<object>();
    [Export] public string ObjectType { get; set; }
    [JsonIgnore] public IActiveItem Object { get; set; }
    public List<object> Parameters { get => Object?.GetParameters() ?? new List<object>(); 
        set 
        {
            _parameters = value;
            Object?.SetParameters(value);
        } 
    }

    public ActiveItem() : base() { }

    public ActiveItem(int id, int maxCount, string itemName, string description, string type) : base(id, maxCount, itemName, description)
    {
        ObjectType = type;
        UpdateItem();
    }

    public void UpdateItem()
    {
        Type itemType = Type.GetType($"{ObjectType}, {Assembly.GetExecutingAssembly().FullName}");
        Object = (IActiveItem)Activator.CreateInstance(itemType);
        if (_parameters.Count > 0)
            Object.SetParameters(_parameters);
    }

    public override object Clone()
    {
        Item item = new ActiveItem(ID, MaxCount, Name, Description, ObjectType);
        item.Count = Count;
        return item;
    }
}