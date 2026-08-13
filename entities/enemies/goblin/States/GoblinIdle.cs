using Godot;
using System;

public partial class GoblinIdle : State
{
    [Export] private Goblin enemy;
    [Export] private AnimatedSprite2D Anims;

    private float idleTimer;

    private CharacterBody2D player;
	public override void Entry()
	{
        player = GetNode<CharacterBody2D>("/root/Overworld/Player");

        Vector2 dir = enemy.LastDirection;
        if (dir == Vector2.Left) Anims.Play("WalkingLeft");
        if (dir == Vector2.Right) Anims.Play("WalkingRight");
        if (dir == Vector2.Up) Anims.Play("WalkingUp");
        if (dir == Vector2.Down) Anims.Play("WalkingDown");

        Anims.Stop();
        Anims.Frame = 0;

        idleTimer = GD.RandRange(1, 4);

        enemy.Velocity = Vector2.Zero;
    }

    public override void Update(float delta)
    {
        Vector2 direction = player.GlobalPosition - enemy.GlobalPosition;
        if (direction.Length() < 80)
        {
            StateMachine.StateChange(this, "GoblinChase");
            return;
        }

        if (idleTimer > 0)
        {
            idleTimer -= delta;
        }
        else
        {
            StateMachine.StateChange(this, "GoblinWander");
        }
    }
}
