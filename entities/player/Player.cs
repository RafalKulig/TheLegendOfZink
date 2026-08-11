using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
	public Vector2 LastDirection { get; private set; } = Vector2.Right;

	private HashSet<ItemToUnlock.UnlockType> Unlocks = new();

	public Dictionary<Enums.EquipmentSlot, IWeapon> Eq = new();

	public Enums.EquipmentSlot ActiveSlot { get; set; } = Enums.EquipmentSlot.SlotA;

	public override void _Ready()
	{
		Eq[Enums.EquipmentSlot.SlotA] = new Sword();
		Eq[Enums.EquipmentSlot.SlotB] = new Shield();
	}

	public override void _Process(double delta)
	{
		if(Velocity != Vector2.Zero)
		{
			LastDirection = Velocity.Normalized();
		}
	}
	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
	}

	public void ItemUnlock(ItemToUnlock.UnlockType ItemType)
	{
		Unlocks.Add(ItemType);
		GD.Print("odblokowano: " +  ItemType.ToString());
	}

	public bool CheckIfUnlocked(ItemToUnlock.UnlockType itemType)
	{
		return Unlocks.Contains(itemType);
	}

	public bool CanAttack(Enums.EquipmentSlot slot)
	{
		IWeapon weapon;
		if(Eq.TryGetValue(slot, out weapon) && weapon is not null)
		{
			return weapon.CanUse(this);
		}
		return false;
	}
}
