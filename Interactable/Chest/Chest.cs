using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

[Tool]
public partial class Chest : StaticBody2D, IInteractable
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
    [Export] private Array<Enums.ItemType> ExcludedItems = new () { Enums.ItemType.KEY };
    [Export] private Enums.ItemType LootItemType;
    [Export] public int LootCount { get; private set; }

    [ExportGroup("Reset Settings")]
    [Export] private bool CanReset = false;
    [Export] private float ResetTime = 300;

    [Export] private Timer ResetTimer;
    [Export] private Node2D LootComponent;
    private LootComponent _LootComponent;

    private bool IsOpen = false;

    public override void _Ready()
    {
        //if (Engine.IsEditorHint()) return;

        if (ResetTimer is not null)
        {
            ResetTimer.Timeout += OnResetTimerTimeout;
        }

        _LootComponent = LootComponent as LootComponent;

        if (_LootComponent is not null)
        {
            _LootComponent.ExcludedTypes = ExcludedItems;

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
        if (IsOpen || player.GlobalPosition.Y < GlobalPosition.Y) return;

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
            LootItemType = _LootComponent.GetRandomItemType();
            LootCount = _LootComponent.GetRandomItemCount(1, 5);
        }

        player.LootToDisplay(LootItemType);
        player.Inventory.AddToItemCount(LootItemType, LootCount);
    }

    private void OnResetTimerTimeout()
    {
        IsOpen = false;
        UpdateSprite();
    }
}
