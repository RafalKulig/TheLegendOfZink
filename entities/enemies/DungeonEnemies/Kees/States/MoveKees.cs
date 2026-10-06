using Godot;
using System;

public partial class MoveKees : State
{
    [Export] private Kees Enemy;
    [Export] private RayCast2D WallCheck;
    [Export] private float MaxSpeed;
    [Export] private float Acceleration;
    [Export] private float Deceleration;

    [Export] private AnimatedSprite2D Anims;
    [Export] private Timer FlightTimer;
    [Export] private Timer DirectionTimer;

    [Export] private float MinAnimSpeed;
    [Export] private float MaxAnimSpeed;

    private Vector2 MoveDirection;
    private float CurrentSpeed;

    private enum Phase { Accelerating, Decelerating, Flying }
    private Phase CurrentPhase;

    private void RandomizeMovement()
    {
        if (FlightTimer is null || DirectionTimer is null) return;

        double Time = GD.Randfn(1.25, 0.75);
        if (Time > 0) DirectionTimer.WaitTime = Time;

        uint Rand = GD.Randi() % 8;

        MoveDirection = Rand switch
        {
            0 => MoveDirection = Vector2.Left,
            1 => MoveDirection = Vector2.Up,
            2 => MoveDirection = Vector2.Right,
            3 => MoveDirection = Vector2.Down,
            4 => new Vector2(1, 1).Normalized(),
            5 => new Vector2(-1, 1).Normalized(),
            6 => new Vector2(1, -1).Normalized(),
            7 => new Vector2(-1, -1).Normalized(),
            _ => MoveDirection = Vector2.Zero
        };

        DirectionTimer.Start();
    }

    private void OnFlightTimeout()
    {
        CurrentPhase = Phase.Decelerating;
    }

    private void OnDirectionTimeout()
    {
        RandomizeMovement();
    }

    public override void Entry()
    {
        FlightTimer.Timeout += OnFlightTimeout;
        DirectionTimer.Timeout += OnDirectionTimeout;

        Anims.Play("default");
        RandomizeMovement();

        CurrentSpeed = 0f;
        CurrentPhase = Phase.Accelerating;

        double Time = GD.Randfn(5, 1);
        if (Time > 0) FlightTimer.WaitTime = Time;

        FlightTimer.Start();
    }

    public override void PhysicsUpdate(float delta)
    {
        if (Enemy is null) return;

        if ((Enemy.GlobalPosition - Enemy.SpawnPos).Length() > 100)
        {
            RandomizeMovement();
        }

        if (WallCheck is not null && WallCheck.IsColliding())
        {
            RandomizeMovement();
        }

        PhaseUpdtae(delta);

        Enemy.Velocity = MoveDirection.Normalized() * CurrentSpeed;

        WallCheck.Rotation = MoveDirection.Angle();

        float speedRatio = MaxSpeed > 0 ? (CurrentSpeed / MaxSpeed) : 0f;
        Anims.SpeedScale = Mathf.Lerp(MinAnimSpeed, MaxAnimSpeed, speedRatio);
    }

    private void PhaseUpdtae(float delta)
    {
        switch (CurrentPhase)
        {
            case Phase.Accelerating:
                CurrentSpeed = Mathf.MoveToward(CurrentSpeed, MaxSpeed, Acceleration * delta);

                if (Mathf.IsEqualApprox(CurrentSpeed, MaxSpeed))
                {
                    CurrentPhase = Phase.Flying;
                }
                break;

            case Phase.Decelerating:
                CurrentSpeed = Mathf.MoveToward(CurrentSpeed, 0, Deceleration * delta);

                if (Mathf.IsEqualApprox(CurrentSpeed, 0f))
                {
                    StateMachine.StateChange(this, "IdleKees");
                }

                break;

            case Phase.Flying:
                CurrentSpeed = MaxSpeed;
                break;
        }
    }

    public override void Exit()
    {
        FlightTimer.Timeout -= OnFlightTimeout;
        FlightTimer.Stop();
        DirectionTimer.Timeout -= OnDirectionTimeout;
        DirectionTimer.Stop();
    }

}
