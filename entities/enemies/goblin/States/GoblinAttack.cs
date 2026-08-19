using Godot;
using System;

public partial class GoblinAttack : State
{
    [Export] private Goblin enemy;
    [Export] private AnimationPlayer Anims;
    private Vector2 AttackDirection;
    private CharacterBody2D player;

    public override void Entry()
    {
        enemy.KnockbackProtection = true;

        player = GetNode<CharacterBody2D>("/root/Overworld/Player");
        Vector2 direction = player.GlobalPosition - enemy.GlobalPosition;
        if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
        {
            if (direction.X >= 0)
            {
                AttackDirection = Vector2.Right;
            }
            else
            {
                AttackDirection = Vector2.Left;
            }
        }
        else
        {
            if (direction.Y >= 0)
            {
                AttackDirection = Vector2.Down;
            }
            else
            {
                AttackDirection = Vector2.Up;
            }
        }
        
        PickAnimation(AttackDirection);
    }

    public void OnAnimationFinished()
    {
        enemy.KnockbackProtection = false;
        StateMachine.StateChange(this, "GoblinWander");
    }

    public void PickAnimation(Vector2 dir)
    {
        if (dir == Vector2.Right)
        {
            Anims.Play("AttackRight");
        }
        if (dir == Vector2.Left)
        {
            Anims.Play("AttackLeft");
        }
        if (dir == Vector2.Down)
        {
            Anims.Play("AttackDown");
        }
        if (dir == Vector2.Up)
        {
            Anims.Play("AttackUp");
        }
    }
}
