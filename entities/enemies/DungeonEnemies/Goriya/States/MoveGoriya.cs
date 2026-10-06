using Godot;
using System;

public partial class MoveGoriya : State
{
    [Export] private Goriya Enemy;
    [Export] private int Speed;
    [Export] private AnimatedSprite2D Anims;
    [Export] private Timer MovementTimer;
    [Export] private Timer ThrowTimer;
    [Export] private RayCast2D WallCheck;

    private Vector2 MoveDirection;

    private void RandomizeMovement()
    {
        if (MovementTimer is null) return;

        double Time = GD.Randfn(1, 0.75);
        if (Time > 0) MovementTimer.WaitTime = Time;

        uint Rand = GD.Randi() % 4;

        MoveDirection = Rand switch
        {
            0 => MoveDirection = Vector2.Left,
            1 => MoveDirection = Vector2.Up,
            2 => MoveDirection = Vector2.Right,
            3 => MoveDirection = Vector2.Down,
            _ => MoveDirection = Vector2.Zero
        };

        DirectionToAnimation();

        MovementTimer.Start();
    }

    public void OnMovementTimeout()
    {
        RandomizeMovement();
    }

    public void OnThrowTimeout()
    {
        StateMachine.StateChange(this, "ThrowGoriya");
    }

    private void DirectionToAnimation()
    {
        if (MoveDirection == Vector2.Left)
        {
            Anims.Play("Left");
        }
        else if (MoveDirection == Vector2.Right)
        {
            Anims.Play("Right");
        }
        else if (MoveDirection == Vector2.Up)
        {
            Anims.Play("Up");
        }
        else if (MoveDirection == Vector2.Down)
        {
            Anims.Play("Down");
        }
    }

    public override void Entry()
    {
        MovementTimer.Timeout += OnMovementTimeout;
        ThrowTimer.Timeout += OnThrowTimeout;
        RandomizeMovement();

        double Time = GD.Randfn(3, 2);
        if (Time > 0) ThrowTimer.WaitTime = Time;

        ThrowTimer.Start();
    }

    public override void PhysicsUpdate(float delta)
    {
        if (Enemy is null) return;

        if (WallCheck is not null && WallCheck.IsColliding())
        {
            RandomizeMovement();
        }

        Enemy.Velocity = MoveDirection.Normalized() * Speed;

        WallCheck.Rotation = MoveDirection.Angle();
    }

    public override void Exit()
    {
        MovementTimer.Timeout -= OnMovementTimeout;
        ThrowTimer.Timeout -= OnThrowTimeout;
    }


}
