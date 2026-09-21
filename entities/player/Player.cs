using Godot;
using System;
using System.Collections.Generic;

//To do -> some sort of animation component
public partial class Player : CharacterBody2D
{
    [Export] public Sprite2D LootItemSprite;

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

        if (Inventory is not null)
        {
            Inventory.WeaponUnlocked += OnWeaponUnlocked;
        }

        WorldEvents.Instance.ChestOpened += OnChestOpen;

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
        GD.Print(DamageDealer.Name);
        IsKnockdbackActive = true;
        KnockbackVelocity = DamageDealer.HitDirection * DamageDealer.KnockbackPower;
        EffectsAnimPlayer.Play("Hit");
        GD.Print(KnockbackVelocity);
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

    private async void OnWeaponUnlocked(ItemToUnlock type)
    {
        StateMachine StateMachine = GetNode<StateMachine>("StateMachine");
        StateMachine.ForceStateChange("Locked");
        EffectsAnimPlayer.Play("Unlock");

        type.GlobalPosition = GlobalPosition + new Vector2(0, -25);

        await ToSignal(EffectsAnimPlayer, AnimationPlayer.SignalName.AnimationFinished);
        type.QueueFree();
    }

    private void OnChestOpen()
    {
        EffectsAnimPlayer.Play("OpenChest");
    }

    public void LootToDisplay(Enums.ItemType item)
    {
        LootItemSprite.Texture = ItemDatabase.GetTexture(item);
        if (item == Enums.ItemType.COIN)
        {
            LootItemSprite.Hframes = 4;
        }
        else
        {
            LootItemSprite.Hframes = 1;
        }
    }

    public void ForceLockState(bool Lock)
    {
        StateMachine StateMachine = GetNode<StateMachine>("StateMachine");
        if (Lock) StateMachine.ForceStateChange("Locked");
        else StateMachine.ForceStateChange("Idle");
    }
}
