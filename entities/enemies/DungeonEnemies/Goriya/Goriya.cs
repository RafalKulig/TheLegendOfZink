using Godot;
using System;
using System.Threading.Tasks;

public partial class Goriya : Enemy
{
    [Export] public PackedScene ProjectileScene { get; private set; }

    public override void _PhysicsProcess(double delta)
    {
        if (Velocity != Vector2.Zero)
        {
            LastDirection = Velocity.Normalized();
        }

        ApplyKnockback((float)delta);

        MoveAndSlide();
    }

    public override void OnEnemyDamaged(int amount, Hitbox DamageDealer)
    {
        IsKnockdbackActive = true;

        KnockbackVelocity = DamageDealer.KnockbackPower * DamageDealer.HitDirection;

        EffectsAnimPlayer.Play("Hit");
    }
}
