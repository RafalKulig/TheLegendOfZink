using Godot;
using System;
using System.Threading.Tasks;

public partial class Aquamentus : Enemy
{
    [Export] private AnimatedSprite2D Animations;
    [Export] private AnimationPlayer BehaviourAnims;
    [Export] private Timer MovementTimer;
    [Export] private Timer AttackTimer;
    [Export] private Hitbox BodyHitbox;
    [Export] private int Speed;
    [Export] private Marker2D Marker;
    [Export] private PackedScene FireballScene;
    [Export] private DoorLock LootRoom;

    private Vector2 MoveDirection;
    private Vector2 MaxLeft, MaxRight;
    private int Distance = 16;

    private bool IsDead = false;

    public override void _Ready()
    {
        base._Ready();

        MovementTimer.Timeout += OnMovementTimeout;
        AttackTimer.Timeout += OnAttackTimeout;

        MaxLeft = new Vector2(SpawnPos.X - Distance, SpawnPos.Y);
        MaxRight = new Vector2(SpawnPos.X + Distance, SpawnPos.Y);
        MoveDirection = Vector2.Left;

        RandomizeTimer(MovementTimer);
        RandomizeTimer(AttackTimer);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IsDead) return;

        if (Velocity != Vector2.Zero)
        {
            LastDirection = Velocity.Normalized();
            BodyHitbox.HitDirection = LastDirection;
        }
        
        if (GlobalPosition.DistanceTo(MaxLeft) <= 0.08 || GlobalPosition.DistanceTo(MaxRight) <= 0.08)
        {
            MoveDirection *= -1;
        }

        Velocity = MoveDirection * Speed;

        MoveAndSlide();
    }

    private void OnMovementTimeout()
    {
        RandomizeMove();
        RandomizeTimer(MovementTimer);
    }

    private void OnAttackTimeout()
    {
        BehaviourAnims.Play("Attack");
        RandomizeTimer(AttackTimer);
    }

    private void RandomizeTimer(Timer Timer)
    {
        if (Timer is null) return;

        double Time = GD.Randfn(3, 0.75);
        if (Time > 0) Timer.WaitTime = Time;

        Timer.Start();
    }

    private void RandomizeMove()
    {
        uint Rand = GD.Randi() % 2;

        MoveDirection = Rand switch
        {
            0 => MoveDirection = Vector2.Left,
            1 => MoveDirection = Vector2.Right,
            _ => MoveDirection = Vector2.Zero
        };
    }

    private void Attack()
    {
        if (IsDead) return;

        for (int i = 0; i < 3; i++)
        {
            Fireball SpawnedFireball = FireballScene.Instantiate<Fireball>();

            GetTree().GetFirstNodeInGroup("Game").AddChild(SpawnedFireball);

            SpawnedFireball.GlobalPosition = Marker.GlobalPosition;
            SpawnedFireball.Direction = (Player.GlobalPosition - Marker.GlobalPosition).Normalized();

            if (i == 1)
            {
                SpawnedFireball.GlobalPosition = SpawnedFireball.GlobalPosition + new Vector2(0, -10);
                SpawnedFireball.Direction = SpawnedFireball.Direction.Rotated(Mathf.DegToRad(20));
            }
            else if (i == 2)
            {
                SpawnedFireball.GlobalPosition = SpawnedFireball.GlobalPosition + new Vector2(0, 10);
                SpawnedFireball.Direction = SpawnedFireball.Direction.Rotated(Mathf.DegToRad(-20));
            }
        }
    }

    public override void OnEnemyDamaged(int amount, Hitbox DamageDealer)
    {
        EffectsAnimPlayer.Play("Hit");
    }

    public override async void OnEnemyDied()
    {
        IsDead = true;

        Velocity = Vector2.Zero;

        EffectsAnimPlayer.Play("Death");

        await ToSignal(EffectsAnimPlayer, AnimationPlayer.SignalName.AnimationFinished);
        LootRoom.CallDeferred(MethodName.QueueFree);
        CallDeferred(MethodName.QueueFree);
    }
}
