using Godot;
using System;

public partial class InventoryUi : Control
{
	[Export] private Player Player; //for InventoryComponent
	[Export] private PackedScene WeaponSlotPrefab;
    [Export] private PackedScene UseableSlotPrefab;

    //UpperPart
    [Export] private HUD HUDPart;

	//LowerPart
	[Export] private GridContainer UnlockedWeaponsGrid;
    [Export] private GridContainer Useables;

    [Export] private Label KeyLabel;

    private InventorySlot SelectedSlot;

    public override void _Ready()
    {
        if (Player is null)
        {
            Player = GetTree().GetFirstNodeInGroup("Player") as Player;
        }

        if (Player is not null)
        {
            Player.Inventory.InventoryUpdated += RefreshUI;
            Visible = !Visible;
            RefreshUI();
        }
    }

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if(@event.IsActionPressed("Inventory") && !@event.IsEcho())
        {
            Visible = !Visible;
            EventBusUI.Instance.EmitSignal(EventBusUI.SignalName.UIVisibilityChanged, "Inventory", Visible);
            GetTree().Paused = !GetTree().Paused;
        }
    }

    public void RefreshUI()
    {
        HUDPart.RefreshUI();

        KeyLabel.Text = "x" + Player.Inventory.GetItemCount(Enums.ItemType.KEY).ToString();

        foreach (Node child in UnlockedWeaponsGrid.GetChildren())
        {
            child.QueueFree();
        }

        foreach (Node child in Useables.GetChildren())
        {
            child.QueueFree();
        }

        foreach (var weapon in Player.Inventory.GetUnlocked())
        {
            var NewSlot = WeaponSlotPrefab.Instantiate<WeaponSlot>();

            Texture2D texture = Player.Inventory.GetTextureFromUnlocked(weapon);

            UnlockedWeaponsGrid.AddChild(NewSlot);

            NewSlot.WeaponType = weapon;
            NewSlot.UpdateSlot(texture, (int)weapon);

            NewSlot.SlotClicked += OnSlotClicked;
        }

        foreach (var Item in Player.Inventory.GetItems())
        {
            if (Item.Key == Enums.ItemType.ARROW || Item.Key == Enums.ItemType.MONEY || Item.Key == Enums.ItemType.BOMB || Item.Key == Enums.ItemType.KEY) continue;

            if (Item.Value <= 0) continue;

            var NewSlot = UseableSlotPrefab.Instantiate<UseableSlot>();

            Texture2D texture = ItemDatabase.GetTexture(Item.Key);

            Useables.AddChild(NewSlot);

            NewSlot.Type = Item.Key;
            NewSlot.UpdateSlot(texture, (int)Item.Key, Item.Value.ToString());

            NewSlot.SlotClicked += OnUseableClicked;
        }
    }

    private void OnSlotClicked(InventorySlot ClickedSlot, int slot)
    {
        var SelectedSlot = ClickedSlot as WeaponSlot;
        Player.Inventory.EquipWeaponToSlot(SelectedSlot.WeaponType, (Enums.EquipmentSlot)slot);
        RefreshUI();
    }

    private void OnUseableClicked(InventorySlot ClickedSlot, int type)
    {
        Enums.ItemType _type = (Enums.ItemType)type;

        switch(_type)
        {
            case Enums.ItemType.HEALTH_POTION:
                Player.Inventory.UseItem(_type, 1);
                break;

            default:
                break;
        }

        RefreshUI();
    }
}
