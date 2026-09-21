using Godot;
using System;
using System.Threading.Tasks;

[GlobalClass]
public partial class Collectable : Area2D
{
    [Export]
    public Enums.ItemType Type { get; private set; }

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    public void OnBodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            AddToInventory(player, 1);
        }
    }

    private void AddToInventory(Player player, int count)
    {
        player.Inventory.AddToItemCount(Type, count);
        CallDeferred(MethodName.QueueFree);
    }
}
