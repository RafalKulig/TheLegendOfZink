using Godot;
using System;
using System.Threading.Tasks;

public partial class Goblin : CharacterBody2D
{
    [Export] private HealthComponent healthComponent;
    [Export] private AnimationPlayer EffectsAnimPlayer;

    public Vector2 LastDirection { get; private set; }

    private Vector2 KnockbackVelocity = Vector2.Zero;
    private bool IsKnockdbackActive = false;
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
            
        }

        ApplyKnockback((float)delta);

        MoveAndSlide();
    }

    private void ApplyKnockback(float delta)
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
        Velocity = Vector2.Zero;

        var StateMachine = GetNode<StateMachine>("StateMachine");
        StateMachine.ProcessMode = ProcessModeEnum.Disabled;

        EffectsAnimPlayer.Play("Death");

        await ToSignal(EffectsAnimPlayer, AnimationPlayer.SignalName.AnimationFinished);
        QueueFree();
    }
}
