using Godot;
using System;

public partial class InventoryUi : Control
{
	[Export] private Player Player; //for InventoryComponent
	[Export] private PackedScene SlotPrefab;

	//UpperPart
	[Export] private TextureRect SlotA;
    [Export] private TextureRect SlotB;
	[Export] private Label CoinsLabel;
    [Export] private Label ArrowsLabel;
    [Export] private Label BombsLabel;

	//LowerPart
	[Export] private GridContainer UnlockedWeaponsGrid;

    private InventorySlot SelectedSlot;

    public override void _Ready()
    {
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

    private void RefreshUI()
    {
        IWeapon WeaponA = Player.Inventory.GetWeaponFromSlot(Enums.EquipmentSlot.SlotA);
        Texture2D WeaponATexture = Player.Inventory.GetTextureFromWeapon(WeaponA);
        IWeapon WeaponB = Player.Inventory.GetWeaponFromSlot(Enums.EquipmentSlot.SlotB);
        Texture2D WeaponBTexture = Player.Inventory.GetTextureFromWeapon(WeaponB);

        if (WeaponATexture is not null)
        {
            SlotA.Texture = WeaponATexture;
            SlotA.Visible = true;
        }
        else
        {
            SlotA.Visible = false;
        }

        if (WeaponBTexture is not null)
        {
            SlotB.Texture = WeaponBTexture;
            SlotB.Visible = true;
        }
        else
        {
            SlotB.Visible = false;
        }

        CoinsLabel.Text = "x" + Player.Inventory.GetItemCount(Enums.ItemType.MONEY).ToString();
        ArrowsLabel.Text = "x" + Player.Inventory.GetItemCount(Enums.ItemType.ARROW).ToString();
        BombsLabel.Text = "x" + Player.Inventory.GetItemCount(Enums.ItemType.BOMB).ToString();

        foreach (Node child in UnlockedWeaponsGrid.GetChildren())
        {
            child.QueueFree();
        }

        foreach (var weapon in Player.Inventory.GetUnlocked())
        {
            var NewSlot = SlotPrefab.Instantiate<InventorySlot>();
            UnlockedWeaponsGrid.AddChild(NewSlot);
            Texture2D texture = Player.Inventory.GetTextureFromUnlocked(weapon);
            NewSlot.UpdateSlot(texture, weapon);

            NewSlot.SlotClicked += OnSlotClicked;
        }
    }

    private void OnSlotClicked(InventorySlot ClickedSlot, Enums.EquipmentSlot slot)
    {
        SelectedSlot = ClickedSlot;
        Player.Inventory.EquipWeaponToSlot(ClickedSlot.ItemType, slot);
        RefreshUI();
    }
}
