using Godot;
using System;

public partial class Slime : Enemy
{
    [Export] private HealthComponent HealthComponent;
    [Export] private Hitbox BodyHitbox;
    [Export] private AnimationPlayer EffectsAnimPlayer;

    public override void _Ready()
    {
        HealthComponent.Died += OnSlimeDied;
        HealthComponent.Damaged += OnSlimeDamaged;
    }

    public override void _PhysicsProcess(double delta)
    {
        if(Velocity != Vector2.Zero) 
        {
            LastDirection = Velocity.Normalized();
            BodyHitbox.HitDirection = LastDirection;
        }

        ApplyKnockback((float)delta);

        MoveAndSlide();
    }

    private async void OnSlimeDied()
    {
        EmitSignal(SignalName.Died, this);

        Velocity = Vector2.Zero;

        EffectsAnimPlayer.Play("Death");

        await ToSignal(EffectsAnimPlayer, AnimationPlayer.SignalName.AnimationFinished);
        QueueFree();
    }

    private void OnSlimeDamaged(int amount, Hitbox DamageDealer)
    {
        IsKnockdbackActive = true;

        KnockbackVelocity = DamageDealer.HitDirection * DamageDealer.KnockbackPower;

        EffectsAnimPlayer.Play("Hit");
    }
}

