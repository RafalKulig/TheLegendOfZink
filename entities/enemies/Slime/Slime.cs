using Godot;
using System;

public partial class Slime : Enemy
{
    [Export] private Hitbox BodyHitbox;

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

    public override async void OnEnemyDied()
    {
        EmitSignal(SignalName.Died, this);

        Velocity = Vector2.Zero;

        EffectsAnimPlayer.Play("Death");

        await ToSignal(EffectsAnimPlayer, AnimationPlayer.SignalName.AnimationFinished);
        QueueFree();
    }

    public override void OnEnemyDamaged(int amount, Hitbox DamageDealer)
    {
        IsKnockdbackActive = true;

        KnockbackVelocity = DamageDealer.HitDirection * DamageDealer.KnockbackPower;

        EffectsAnimPlayer.Play("Hit");
    }
}

