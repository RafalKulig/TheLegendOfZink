using Godot;
using System;

[GlobalClass]
public partial class Enemy : CharacterBody2D
{
    [Signal] public delegate void DiedEventHandler(Enemy enemy);
    [Export] public HealthComponent HealthComponent { get; private set; }
    [Export] public AnimationPlayer EffectsAnimPlayer { get; private set; }
    [Export] public LootComponent LootComponent { get; private set; }
    private Player _player;
    public Player Player 
    {
        get
        { 
            if (_player is null && IsInsideTree())
            {
                _player = GetTree().GetFirstNodeInGroup("Player") as Player;
            }
            return _player;
        }
        private set
        {
            _player = value;
        }
    }
    public Vector2 LastDirection { get; protected set; }
    protected Vector2 KnockbackVelocity = Vector2.Zero;
    protected bool IsKnockdbackActive = false;

    public Vector2 SpawnPos;

    public override void _Ready()
    {
        Player = GetTree().GetFirstNodeInGroup("Player") as Player;

        HealthComponent.Died += OnEnemyDied;
        HealthComponent.Damaged += OnEnemyDamaged;
        SpawnPos = GlobalPosition;
    }

    public virtual void OnEnemyDamaged(int amount, Hitbox DamageDealer) { }

    public virtual async void OnEnemyDied()
    {
        if (LootComponent is not null) LootComponent.SpawnRandomLoot();

        Velocity = Vector2.Zero;

        EffectsAnimPlayer.Play("Death");

        await ToSignal(EffectsAnimPlayer, AnimationPlayer.SignalName.AnimationFinished);
        CallDeferred(MethodName.QueueFree);
    }

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
