using Godot;
using System;

public partial class GoblinWander : State
{
    [Export] private Goblin enemy;
    [Export] private AnimatedSprite2D Anims;
    [Export] private int speed = 10;

    Vector2 moveDirection;
    float wanderTime;

    private CharacterBody2D player;

    private void randomizeWander()
    {
        int random = GD.RandRange(0, 4);
        wanderTime = GD.RandRange(1, 3);
        switch (random)
        {
            case 0:
                moveDirection = Vector2.Left;
                break;
            case 1:
                moveDirection = Vector2.Up;
                break;
            case 2:
                moveDirection = Vector2.Right;
                break;
            case 3:
                moveDirection = Vector2.Down;
                break;
            case 4:
                moveDirection = Vector2.Zero;
                wanderTime = 0;
                break;
        }
    }

    public override void Entry()
    {
        player = GetNode<CharacterBody2D>("/root/Overworld/Player");
        randomizeWander();
    }

    public override void Exit()
    {

    }

    public override void Update(float delta)
    {
        Vector2 direction = player.GlobalPosition - enemy.GlobalPosition;
        if (direction.Length() < 80)
        {
            StateMachine.StateChange(this, "GoblinChase");
            return;
        }

        if (wanderTime > 0)
        {
            wanderTime -= delta;
        }
        else
        {
            if (moveDirection == Vector2.Zero)
            {
                StateMachine.StateChange(this, "GoblinIdle");
                return;
            }
            randomizeWander();
        }

        WalkingAnimation(moveDirection);
    }

    public override void PhysicsUpdate(float delta)
    {
        if(enemy is not null)
        {
            enemy.Velocity = moveDirection * speed;
        }
    }

    private void WalkingAnimation(Vector2 dir)
    {
        if (dir == Vector2.Left)
            Anims.Play("WalkingLeft");
        if (dir == Vector2.Right)
            Anims.Play("WalkingRight");
        if (dir == Vector2.Up)
            Anims.Play("WalkingUp");
        if (dir == Vector2.Down)
            Anims.Play("WalkingDown");
    }
}
