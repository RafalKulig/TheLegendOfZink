using Godot;
using System;

public partial class GoblinIdle : State
{
    [Export] private CharacterBody2D enemy;
    [Export] private AnimatedSprite2D Anims;

    private float idleTimer;

    private CharacterBody2D player;
	public override void Entry()
	{
        player = GetNode<CharacterBody2D>("/root/Overworld/Player");

        idleTimer = GD.RandRange(1, 4);
        Anims.Stop();
        Anims.Frame = 0;
        enemy.Velocity = Vector2.Zero;
    }

    public override void Exit()
    {
        //to do -> animacja wyjscia prawo/lewo
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
