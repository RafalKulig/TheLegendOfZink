using Godot;
using System;

public partial class GoblinChase : State
{
    [Export] private Goblin enemy;
    [Export] private AnimatedSprite2D Anims;
    [Export] private int speed = 20;
	private CharacterBody2D player;


    private bool preferXMovement = true;
    private float AxisTolerence = 5.0f;

	public override void Entry()
	{
        player = GetNode<CharacterBody2D>("/root/Overworld/Player");
        Vector2 direction = player.GlobalPosition - enemy.GlobalPosition;
        preferXMovement = Mathf.Abs(direction.X) > Mathf.Abs(direction.Y);
    }

    public override void PhysicsUpdate(float delta)
    {
        Vector2 direction = player.GlobalPosition - enemy.GlobalPosition;
        float distance = direction.Length();

        if (distance > 100)
        {
            StateMachine.StateChange(this, "GoblinIdle");
            return;
        }

        if (distance > 20)
        {
            if (preferXMovement)
            {
                if (Mathf.Abs(direction.X) < AxisTolerence)
                {
                    preferXMovement = false;
                }
            }
            else
            {
                if (Mathf.Abs(direction.Y) < AxisTolerence)
                {
                    preferXMovement = true;
                }
            }

            if (preferXMovement)
            {
                enemy.Velocity = new Vector2(direction.X, 0).Normalized() * speed;
            }
            else
            {
                enemy.Velocity = new Vector2(0, direction.Y).Normalized() * speed;
            }
        }
        else
        {
            enemy.Velocity = Vector2.Zero;
            StateMachine.StateChange(this, "GoblinAttack");
            return;
        }

        WalkingAnimation(enemy.Velocity.Normalized());
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
