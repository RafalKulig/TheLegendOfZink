using Godot;
using System;
using System.Threading.Tasks;

[GlobalClass]
public partial class Collectable : Area2D
{
    [Export]
    public Enums.ItemType Type { get; protected set; }
    protected int count = 1;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    public void OnBodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            AddToInventory(player, count);
        }
    }

    private void AddToInventory(Player player, int count)
    {
        player.Inventory.AddToItemCount(Type, count);
        CallDeferred(MethodName.QueueFree);
    }
}
