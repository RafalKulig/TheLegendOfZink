using Godot;
using System;

public partial class Gel : Enemy
{
    [Export] private Hitbox BodyHitbox;

    public override void _Ready()
    {
        base._Ready();
        EffectsAnimPlayer.Play("walk");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Velocity != Vector2.Zero)
        {
            LastDirection = Velocity.Normalized();
            BodyHitbox.HitDirection = LastDirection;
        }

        ApplyKnockback((float)delta);

        MoveAndSlide();
    }

    public override void OnEnemyDamaged(int amount, Hitbox DamageDealer)
    {
        GD.Print("damage");
        IsKnockdbackActive = true;

        KnockbackVelocity = DamageDealer.KnockbackPower * DamageDealer.HitDirection;

        EffectsAnimPlayer.Play("Hit");
    }

    public override async void OnEnemyDied()
    {
        Velocity = Vector2.Zero;

        EffectsAnimPlayer.Play("Death");

        await ToSignal(EffectsAnimPlayer, AnimationPlayer.SignalName.AnimationFinished);
        CallDeferred(MethodName.QueueFree);
    }
}
