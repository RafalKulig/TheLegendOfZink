using Godot;
using System;

public partial class HUD : Control
{
	[Export] private Player Player;

    [Export] private TextureRect SlotA;
    [Export] private TextureRect SlotB;

    [Export] private TextureRect HealthBar;

    [Export] private Label CoinsLabel;
    [Export] private Label ArrowsLabel;
    [Export] private Label BombsLabel;

    public override void _Ready()
    {
        if (Player is not null)
        {
            Player.Inventory.InventoryUpdated += RefreshUI;
            //RefreshUI();
            CallDeferred(MethodName.RefreshUI);
        }

        EventBusUI.Instance.UIVisibilityChanged += OnUiVisibilityChanged;
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

        HealthBarChange();

        CoinsLabel.Text = "x" + Player.Inventory.GetItemCount(Enums.ItemType.MONEY).ToString();
        ArrowsLabel.Text = "x" + Player.Inventory.GetItemCount(Enums.ItemType.ARROW).ToString();
        BombsLabel.Text = "x" + Player.Inventory.GetItemCount(Enums.ItemType.BOMB).ToString();
    }

    private void HealthBarChange()
    {
        int PlayerHealth = Player.healthComponent.currentHealth;

        AtlasTexture Atlas = (AtlasTexture)HealthBar.Texture;

        float NewX = (float)PlayerHealth switch
        {
            5 => 0,
            4 => 40,
            3 => 80,
            2 => 120,
            1 => 160,
            0 => 200,
            _ => 0
        };

        Atlas.Region = new Rect2(NewX, 0, Atlas.Region.Size);
    }

    private void OnUiVisibilityChanged(string UiName, bool IsOpen)
    {
        if (UiName == "Inventory")
        {
            Visible = !IsOpen;
        }
        RefreshUI();
    }
}
