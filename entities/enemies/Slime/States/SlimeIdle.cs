using Godot;
using System;

public partial class SlimeIdle : State
{
	[Export] private Slime Enemy;
    [Export] private AnimatedSprite2D Anims;

    private float IdleTimer;

    public override void Entry()
    {
        Enemy.Velocity = Vector2.Zero;

        IdleTimer = GD.RandRange(1, 3);

        Anims.Play("Idle");
    }

    public override void Update(float delta)
    {
        if (IdleTimer > 0)
        {
            IdleTimer -= delta;

        }
        else if (IdleTimer <= 0)
        {
            StateMachine.StateChange(this, "SlimeMove");
        }
    }

}
