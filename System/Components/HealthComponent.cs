using Godot;
using System;

[GlobalClass]
public partial class HealthComponent : Node2D
{
	[Signal] public delegate void HealthChangedEventHandler(int currentHealth); //might use
	[Signal] public delegate void DamagedEventHandler(int amount);
	[Signal] public delegate void DiedEventHandler();

	[Export] private int maxHealth;

	public int currentHealth;

    public override void _Ready()
    {
		currentHealth = maxHealth;
    }

	public void ReceiveDamage(int amount)
	{
		if (currentHealth <= 0) return;

		currentHealth -= amount;

		EmitSignal(SignalName.Damaged, amount);

		if (currentHealth <= 0)
		{
			EmitSignal(SignalName.Died);
		}
	}
}
