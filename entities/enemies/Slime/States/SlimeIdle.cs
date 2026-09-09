using Godot;
using System;

public partial class SlimeIdle : State
{
	[Export] private Enemy Enemy;
    [Export] private AnimatedSprite2D Anims;

	private Player Player;
    private float IdleTimer;

    public override void Entry()
    {
        Player = GetTree().Root.GetNode<Player>("/root/Overworld/Player");

        Enemy.Velocity = Vector2.Zero;

        IdleTimer = GD.RandRange(1, 3);

        Anims.Play("Idle");
    }

    public override void Update(float delta)
    {
        Vector2 direction = Player.GlobalPosition - Enemy.GlobalPosition;
        if (direction.Length() < 10)
        {
            StateMachine.StateChange(this, "SlimeMove");
            return;
        }

        if (IdleTimer >= 0)
        {
            IdleTimer -= delta;

        }
        else
        {
            StateMachine.StateChange(this, "SlimeMove");
        }
    }

}
