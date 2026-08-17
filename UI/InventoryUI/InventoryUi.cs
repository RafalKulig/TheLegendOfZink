using Godot;
using System;

public partial class InventoryUi : Control
{
	[Export] private Player Player; //for InventoryComponent
	[Export] private PackedScene SlotPrefab;

	//UpperPart
	[Export] private InventorySlot SlotA;
    [Export] private InventorySlot SlotB;
	[Export] private Label CoinsLabel;
    [Export] private Label ArrowsLabel;
    [Export] private Label BombsLabel;

	//LowerPart
	[Export] private GridContainer UnlockedWeaponsGrid;

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
        }
    }

    private void RefreshUI()
    {
        IWeapon WeaponA = Player.Inventory.GetWeaponFromSlot(Enums.EquipmentSlot.SlotA);
        Texture2D WeaponATexture = Player.Inventory.GetTextureFromWeapon(WeaponA);
        IWeapon WeaponB = Player.Inventory.GetWeaponFromSlot(Enums.EquipmentSlot.SlotB);
        Texture2D WeaponBTexture = Player.Inventory.GetTextureFromWeapon(WeaponB);

        if(WeaponATexture is not null) SlotA.UpdateSlot(WeaponATexture);
        if (WeaponBTexture is not null) SlotB.UpdateSlot(WeaponBTexture);

        CoinsLabel.Text = "x" + Player.Inventory.GetItemCount(Enums.ItemType.COIN).ToString();
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
            NewSlot.UpdateSlot(texture);
        }
    }
}
