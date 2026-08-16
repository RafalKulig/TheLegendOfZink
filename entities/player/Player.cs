using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
    [Export] private HealthComponent healthComponent;
	[Export] public InventoryComponent Inventory { get; private set; }

	public Vector2 LastDirection { get; private set; } = Vector2.Right;

	public override void _Ready()
	{
		Inventory.EquipWeaponToSlot(new Sword(), Enums.EquipmentSlot.SlotA);
        Inventory.EquipWeaponToSlot(new Bow(), Enums.EquipmentSlot.SlotB);

        if (healthComponent is not null)
        {
            healthComponent.Died += OnPlayerDied;
            healthComponent.Damaged += OnPlayerDamaged;
        }
        if (Inventory is not null)
        {
            Inventory.ItemCountChanged += OnItemCountChanged;
        }

        Inventory.AddToItemCount(Enums.ItemType.ARROW, 10);
    }

	public override void _PhysicsProcess(double delta)
	{
        if (Velocity != Vector2.Zero)
        {
            LastDirection = Velocity.Normalized();
        }

        if (Input.IsActionJustPressed("Inventory"))
        {
            Inventory.ShowEq();
        }

        MoveAndSlide();
	}

    private void OnPlayerDamaged(int amount)
    {
        GD.Print("ale boli " + Name + "'a " + amount + " zostalo: " + healthComponent.currentHealth);
    }

    private void OnPlayerDied()
    {
        GD.Print("umarl: " + Name);
    }

	private void OnItemCountChanged(Enums.ItemType item, int count)
	{
        GD.Print(item + ": " + count);
	}
}
