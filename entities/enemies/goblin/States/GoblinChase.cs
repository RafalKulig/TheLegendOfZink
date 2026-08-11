using Godot;
using System;

public partial class GoblinChase : State
{
    [Export] private CharacterBody2D enemy;
    [Export] private AnimatedSprite2D Anims;
    [Export] private int speed = 20;
	private CharacterBody2D player;

	public override void Entry()
	{
        player = GetNode<CharacterBody2D>("/root/Overworld/Player");
        GD.Print(player);
    }

    public override void PhysicsUpdate(float delta)
    {
        Vector2 direction = player.GlobalPosition - enemy.GlobalPosition;

        if (direction.Length() > 20)
        {
            if (Mathf.FloorToInt(direction.X) != 0)
            {
                enemy.Velocity = new Vector2(direction.X, 0).Normalized() * speed;
            }
            else
            {
                enemy.Velocity = new Vector2(0, direction.Y).Normalized() * speed;
            }
            //enemy.Velocity = direction.Normalized() * speed;
        }
        else
        {
            enemy.Velocity = Vector2.Zero;
        }

        if (direction.Length() > 100)
        {
            StateMachine.StateChange(this, "GoblinIdle");
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
