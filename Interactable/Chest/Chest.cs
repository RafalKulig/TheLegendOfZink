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
    [Export] private PackedScene CustomLootItem;
    [Export] private int LootCount;

    [ExportGroup("Reset Settings")]
    [Export] private bool CanReset = false;
    [Export] private float ResetTime = 300;

    private bool IsOpen = false;

    public override void _Ready()
    {
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (Sprite is null) return;

        int BaseFrame = (int)ChestType * 2;
        Sprite.Frame = IsOpen ? BaseFrame + 1 : BaseFrame;
    }

    public void Interact()
    {
        if (IsOpen) return;

        OpenChest();
    }

    private void OpenChest()
    {
        IsOpen = true;
        UpdateSprite();
        SpawnLoot();

        if (CanReset)
        {
            StartResetTimer();
        }
    }

    private void SpawnLoot()
    {
        GD.Print("POJAWIA SIE LOOT!");
    }

    private async void StartResetTimer()
    {
        await ToSignal(GetTree().CreateTimer(ResetTime), "timeout");

        IsOpen = false;
        UpdateSprite();
    }

}
