using Godot;
using System;
using System.Threading.Tasks;

[GlobalClass]
public partial class Collectable : Area2D
{
    [Export] public Enums.ItemType Type { get; protected set; }
    [Export] protected bool ShouldDespawn = true;
    protected int count = 1;
    [Export] protected Timer Timer;
    [Export] protected double TimeToLive = 120;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;

        if (!ShouldDespawn || Timer is null) return;

        Timer.Timeout += OnTimerTimeout;
        Timer.Start(TimeToLive);
    }

    public void OnBodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            AddToInventory(player, count);
        }
    }

    public void OnTimerTimeout()
    {
        CallDeferred(MethodName.QueueFree);
    }

    protected void AddToInventory(Player player, int count)
    {
        player.Inventory.AddToItemCount(Type, count);
        CallDeferred(MethodName.QueueFree);
    }
}
