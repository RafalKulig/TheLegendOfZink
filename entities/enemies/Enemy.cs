using Godot;
using System;

[GlobalClass]
public partial class Enemy : CharacterBody2D
{
    [Signal] public delegate void DiedEventHandler(Enemy enemy);
    public Vector2 LastDirection { get; protected set; }
    protected Vector2 KnockbackVelocity = Vector2.Zero;
    protected bool IsKnockdbackActive = false;

    protected void ApplyKnockback(float delta)
    {
        if (!IsKnockdbackActive) return;

        KnockbackVelocity = KnockbackVelocity.MoveToward(Vector2.Zero, 1000 * delta);
        if (KnockbackVelocity.Length() > 10 && IsKnockdbackActive)
        {
            Velocity = KnockbackVelocity;
        }
        else if (IsKnockdbackActive)
        {
            Velocity = Vector2.Zero;
            IsKnockdbackActive = false;
        }
    }
}
