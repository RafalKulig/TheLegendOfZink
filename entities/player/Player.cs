using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
    [Export] private HealthComponent healthComponent;
	[Export] public InventoryComponent Inventory { get; private set; }
    [Export] private Hitbox Hitbox;
    [Export] private AnimationPlayer EffectsAnimPlayer;

	public Vector2 LastDirection { get; private set; } = Vector2.Right;

    private Vector2 KnockbackVelocity = Vector2.Zero;
    private bool IsKnockdbackActive = false;
    public bool KnockbackProtection = false;

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

        ApplyKnockback((float)delta);

        MoveAndSlide();
	}

    private void OnPlayerDamaged(int amount, Hitbox DamageDealer)
    {
        IsKnockdbackActive = true;
        KnockbackVelocity = DamageDealer.HitDirection * DamageDealer.KnockbackPower;
        EffectsAnimPlayer.Play("Hit");
    }

    private void OnPlayerDied()
    {
        GD.Print("umarl: " + Name);
    }

    private void ApplyKnockback(float delta)
    {
        if (!IsKnockdbackActive) return;

        KnockbackVelocity = KnockbackVelocity.MoveToward(Vector2.Zero, 1000 * delta);
        if (KnockbackVelocity.Length() > 10 && IsKnockdbackActive)
        {
            Velocity = KnockbackVelocity;
        }
        else if (IsKnockdbackActive)
        {
            Velocity = Vector2.Zero;
            IsKnockdbackActive = false;
        }
    }


}
