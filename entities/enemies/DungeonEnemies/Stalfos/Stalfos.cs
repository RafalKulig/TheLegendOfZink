using Godot;
using System;
using System.Threading.Tasks;

public partial class Stalfos : Enemy
{
    [Export] private Hitbox BodyHitbox;
    [Export] private AnimationPlayer Effects;

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
        IsKnockdbackActive = true;

        KnockbackVelocity = DamageDealer.KnockbackPower * DamageDealer.HitDirection;

        Effects.Play("Hit");
    }

    public override async void OnEnemyDied()
    {
        Velocity = Vector2.Zero;

        Effects.Play("Death");

        await ToSignal(Effects, AnimationPlayer.SignalName.AnimationFinished);
        CallDeferred(MethodName.QueueFree);
    }
}
