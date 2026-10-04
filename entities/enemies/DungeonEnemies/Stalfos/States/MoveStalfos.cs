using Godot;
using System;

public partial class MoveStalfos : State
{
	[Export] private Stalfos Enemy;
    [Export] private int Speed;
	[Export] private AnimatedSprite2D Anims;
	[Export] private Timer Timer;
    [Export] private RayCast2D WallCheck;

    private Vector2 MoveDirection;

    private void RandomizeMovement()
    {
        if (Timer is null) return;

        double Time = GD.Randfn(1, 0.75);
        if (Time > 0) Timer.WaitTime = Time;

        uint Rand = GD.Randi() % 4;

        MoveDirection = Rand switch
        {
            0 => MoveDirection = Vector2.Left,
            1 => MoveDirection = Vector2.Up,
            2 => MoveDirection = Vector2.Right,
            3 => MoveDirection = Vector2.Down,
            _ => MoveDirection = Vector2.Zero
        };

        Timer.Start();
    }

    private void OnTimerTimeout()
    {
        RandomizeMovement();
    }

    public override void Entry()
    {
        Timer.Timeout += OnTimerTimeout;
        Anims.Play("default");
        RandomizeMovement();
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
        Timer.Timeout -= OnTimerTimeout;
    }

}
