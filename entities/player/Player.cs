using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
    [Export] private HealthComponent healthComponent;
	[Export] public InventoryComponent Inventory { get; private set; }
    [Export] private Hitbox Hitbox;

	public Vector2 LastDirection { get; private set; } = Vector2.Right;

    public override void _Ready()
	{
		Inventory.EquipWeaponToSlot(Enums.UnlockType.SWORD, Enums.EquipmentSlot.SlotA);
        Inventory.EquipWeaponToSlot(Enums.UnlockType.BOW, Enums.EquipmentSlot.SlotB);

        if (healthComponent is not null)
        {
            healthComponent.Died += OnPlayerDied;
            healthComponent.Damaged += OnPlayerDamaged;
        }

        Inventory.AddToItemCount(Enums.ItemType.ARROW, 10);
    }

	public override void _PhysicsProcess(double delta)
	{
        if (Velocity != Vector2.Zero)
        {
            LastDirection = Velocity.Normalized();
            Hitbox.HitDirection = LastDirection;
        }

        MoveAndSlide();
	}

    private void OnPlayerDamaged(int amount, Hitbox DamageDealer)
    {
        GD.Print("ale boli " + Name + "'a " + amount + " zostalo: " + healthComponent.currentHealth);
    }

    private void OnPlayerDied()
    {
        GD.Print("umarl: " + Name);
    }


}
