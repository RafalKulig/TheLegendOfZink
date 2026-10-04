using Godot;
using System;

public partial class Kees : Enemy
{

    public override void _PhysicsProcess(double delta)
    {
        ApplyKnockback((float)delta);

        MoveAndSlide();
    }

    public override void OnEnemyDamaged(int amount, Hitbox DamageDealer)
    {
        IsKnockdbackActive = true;

        KnockbackVelocity = DamageDealer.KnockbackPower * DamageDealer.HitDirection;
    }

    public override async void OnEnemyDied()
    {
        Velocity = Vector2.Zero;

        EffectsAnimPlayer.Play("Death");

        await ToSignal(EffectsAnimPlayer, AnimationPlayer.SignalName.AnimationFinished);
        CallDeferred(MethodName.QueueFree);
    }
}
