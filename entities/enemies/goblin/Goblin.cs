using Godot;
using System;
using System.Threading.Tasks;

public partial class Goblin : Enemy
{
    [Export] private Hitbox AttackHitbox, BodyHitbox;

    public bool KnockbackProtection = false;

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

    public override void OnEnemyDamaged(int amount, Hitbox DamageDealer)
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

    public override async void OnEnemyDied()
    {
        EmitSignal(SignalName.Died, this);

        Velocity = Vector2.Zero;

        EffectsAnimPlayer.Play("Death");

        await ToSignal(EffectsAnimPlayer, AnimationPlayer.SignalName.AnimationFinished);
        QueueFree();
    }
}
