using Godot;
using System;

public partial class MoveGel : State
{
    [Export] private Gel Enemy;
    [Export] private int Speed;
    [Export] private RayCast2D WallCheck;

    private Vector2 MoveDirection;
    private Vector2 GoToPos;
    private const int MOVELENGHT = 16;

    private void RandomizeMovement()
    {
        uint Rand = GD.Randi() % 4;

        MoveDirection = Rand switch
        {
            0 => MoveDirection = Vector2.Left,
            1 => MoveDirection = Vector2.Up,
            2 => MoveDirection = Vector2.Right,
            3 => MoveDirection = Vector2.Down,
            _ => MoveDirection = Vector2.Zero
        };

        GoToPos = Enemy.GlobalPosition + MoveDirection * MOVELENGHT;
    }

    public override void Entry()
    {
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

        if (Enemy.GlobalPosition.DistanceTo(GoToPos) <= 0.08)
        {
            StateMachine.StateChange(this, "IdleGel");
        }
    }
}
