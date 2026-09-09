using Godot;
using System;

public partial class SlimeMove : State
{
	[Export] private Enemy Enemy;
    [Export] private AnimatedSprite2D Anims;
    [Export] private int Speed = 20;
    [Export] private int DetectionDistance;
    [Export] private int StopDistance;

    private Player Player;
    private Vector2 DirectionToPlayer;
    private Vector2 MoveDirection;
    private float WanderTimer;

    public override void Entry()
    {
        Player = GetTree().Root.GetNode<Player>("/root/Overworld/Player");
        Anims.Play("Jump");
        MoveDirection = GetRandDir();
        WanderTimer = GD.RandRange(1, 4);
    }

    public override void Update(float delta)
    {
        DirectionToPlayer = Player.GlobalPosition - Enemy.GlobalPosition;
        if (DirectionToPlayer.Length() < DetectionDistance)
        {
            MoveDirection = DirectionToPlayer;
        }
        else if (DirectionToPlayer.Length() > StopDistance)
        {
            StateMachine.StateChange(this, "SlimeIdle");
        }
        else if (WanderTimer > 0)
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
        Enemy.Velocity = MoveDirection.Normalized() * Speed;
    }

    Vector2 GetRandDir()
    {
        double x = GD.RandRange(-1, 1);
        double y = GD.RandRange(-1, 1);
        //double multiplier = GD.RandRange(5, 35);

        Vector2 result = new Vector2((float)x, (float)y);

        return result;
    }
}
