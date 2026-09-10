using Godot;
using System;
using static Godot.SkeletonModifier3D;

public partial class SlimeMove : State
{
    [Export] private Slime Enemy;
    [Export] private AnimatedSprite2D Anims;
    [Export] private int Speed = 15;

    private Vector2 MoveDirection;
    private float WanderTimer;

    public override void Entry()
    {
        Anims.Play("Jump");
        RandomizeMove();
        WanderTimer = GD.RandRange(1, 4);
    }

    public override void Update(float delta)
    {
        if (WanderTimer > 0)
        {
            WanderTimer -= delta;
        }
        else if (WanderTimer <= 0)
        {
            StateMachine.StateChange(this, "SlimeIdle");
        }
    }

    public override void PhysicsUpdate(float delta)
    {
        if (Enemy is null) return;

        Enemy.Velocity = MoveDirection.Normalized() * Speed;
    }

    void RandomizeMove()
    {
        int random = GD.RandRange(0, 3);
        switch (random)
        {
            case 0:
                MoveDirection = Vector2.Left;
                break;
            case 1:
                MoveDirection = Vector2.Up;
                break;
            case 2:
                MoveDirection = Vector2.Right;
                break;
            case 3:
                MoveDirection = Vector2.Down;
                break;
        }
    }
}
