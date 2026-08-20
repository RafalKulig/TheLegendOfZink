using Godot;
using System;

[GlobalClass]
public partial class Collectable : Area2D
{
    [Export]
    public Enums.ItemType Type { get; private set; }

    public override void _Ready()
    {
        this.BodyEntered += OnBodyEntered;
    }

    public void OnBodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            player.Inventory.AddToItemCount(Type, 1);
            QueueFree();
        }
    }
}
