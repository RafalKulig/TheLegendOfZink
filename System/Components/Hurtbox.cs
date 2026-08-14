using Godot;
using System;

[GlobalClass]
public partial class Hurtbox : Area2D
{
	[Export] private HealthComponent healthComponent;


    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
    }

    public override void _ExitTree()
    {
        AreaEntered -= OnAreaEntered;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is Hitbox hitbox)
            TakeDamage(hitbox.Damage);
    }

    public void TakeDamage(int amount)
    {
        if (healthComponent is not null)
        {
            healthComponent.ReceiveDamage(amount);
        }
    }

    //to do
    //TakeDamage(int amount, Vector2 hitDirection)
}
