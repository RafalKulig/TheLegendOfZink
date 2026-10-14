using Godot;
using System;

[GlobalClass]
public partial class HealthComponent : Node2D
{
	[Signal] public delegate void HealthChangedEventHandler(); //might use
	[Signal] public delegate void DamagedEventHandler(int amount, Hitbox DamageDealer);
	[Signal] public delegate void DiedEventHandler();

	[Export] private int maxHealth;

	public int currentHealth;

    public override void _Ready()
    {
		currentHealth = maxHealth;
    }

	public void ReceiveDamage(int Amount, Hitbox DamageDealer)
	{
		if (currentHealth <= 0) return;

		currentHealth -= Amount;

		EmitSignal(SignalName.Damaged, Amount, DamageDealer);
		EmitSignal(SignalName.HealthChanged);

		if (currentHealth <= 0)
		{
			EmitSignal(SignalName.Died);
		}
	}

	public void Heal(int Amount)
	{
		if (IsHealed()) return;

		currentHealth += Amount;

		EmitSignal(SignalName.HealthChanged);
	}

	public bool IsHealed()
	{
		return currentHealth == maxHealth;
	}
}
