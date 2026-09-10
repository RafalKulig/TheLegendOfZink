using Godot;
using System;
using System.Threading.Tasks;

public partial class Goblin : Enemy
{
    [Export] private HealthComponent healthComponent;
    [Export] private AnimationPlayer EffectsAnimPlayer;
    [Export] private Hitbox AttackHitbox, BodyHitbox;

    public bool KnockbackProtection = false;

    public override void _Ready()
    {
        if (healthComponent is not null)
        {
            healthComponent.Died += OnGoblinDied;
            healthComponent.Damaged += OnGoblinDamaged;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Velocity != Vector2.Zero)
        {
            LastDirection = Velocity.Normalized();
            AttackHitbox.HitDirection = LastDirection;
            BodyHitbox.HitDirection = LastDirection;
        }

        ApplyKnockback((float)delta);

        MoveAndSlide();
    }

    private void OnGoblinDamaged(int amount, Hitbox DamageDealer)
    {
        IsKnockdbackActive = true;
        if(KnockbackProtection)
        {
            KnockbackVelocity = DamageDealer.HitDirection * DamageDealer.KnockbackPower/2;
        }
        else 
        {
            KnockbackVelocity = DamageDealer.HitDirection * DamageDealer.KnockbackPower;
        }
        EffectsAnimPlayer.Play("Hit");
    }

    private async void OnGoblinDied()
    {
        EmitSignal(SignalName.Died, this);

        Velocity = Vector2.Zero;

        EffectsAnimPlayer.Play("Death");

        await ToSignal(EffectsAnimPlayer, AnimationPlayer.SignalName.AnimationFinished);
        QueueFree();
    }
}
