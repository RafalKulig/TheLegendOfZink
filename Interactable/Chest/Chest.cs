using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

[Tool]
//TO DO:
//Make Interactable interface
public partial class Chest : StaticBody2D
{
    [ExportGroup("Type & Visiuals")]
    private Enums.ChestType _chestType;
    [Export] private Enums.ChestType ChestType
    {
        get => _chestType;
        set
        {
            _chestType = value;
            UpdateSprite();
        }
    }
    [Export] private Sprite2D Sprite;

    [ExportGroup("Loot Settings")]
    [Export] private bool RandomLoot = true;
    [Export] private Enums.ItemType LootItemType;
    [Export] public int LootCount { get; private set; }

    [ExportGroup("Reset Settings")]
    [Export] private bool CanReset = false;
    [Export] private float ResetTime = 300;

    [Export] private Timer ResetTimer;

    private bool IsOpen = false;

    public override void _Ready()
    {
        if (ResetTimer is not null)
        {
            ResetTimer.Timeout += OnResetTimerTimeout;
        }

        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (Sprite is null) return;

        int BaseFrame = (int)ChestType * 2;
        Sprite.Frame = IsOpen ? BaseFrame + 1 : BaseFrame;
    }

    public void Interact(Player player)
    {
        //Move direction check to InteracionComponent
        if (IsOpen || player.LastDirection != Vector2.Up) return;

        OpenChest(player);
    }

    private void OpenChest(Player player)
    {
        IsOpen = true;
        UpdateSprite();
        SpawnLoot(player);

        WorldEvents.Instance.EmitSignal(WorldEvents.SignalName.ChestOpened);

        if (CanReset && ResetTimer is not null)
        {
            ResetTimer.Start(ResetTime);
        }
    }

    private void SpawnLoot(Player player)
    {
        if (RandomLoot)
        {
            SpawnRandomLoot();
        }

        player.LootToDisplay(LootItemType);
        player.Inventory.AddToItemCount(LootItemType, LootCount);
    }

    private void SpawnRandomLoot()
    {
        uint RandomItem = GD.Randi() % 3;
        uint RandomItemCount = GD.Randi() % 5 + 1;

        LootItemType = (Enums.ItemType)RandomItem;
        LootCount = (int)RandomItemCount;
    }

    private void OnResetTimerTimeout()
    {
        IsOpen = false;
        UpdateSprite();
    }
}
