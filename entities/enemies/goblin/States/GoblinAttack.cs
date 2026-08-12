using Godot;
using System;

public partial class GoblinAttack : State
{
    [Export] private Goblin enemy;
    [Export] private AnimatedSprite2D Anims;
    private float AttackCooldown;
    private bool AttackFinished;

    public override void Entry()
    {
        GD.Print("Entry");
        AttackFinished = false;
        AttackCooldown = 1.0f;
        GD.Print(enemy.LastDirection);
        PickAnimation(enemy.LastDirection);
    }

    public override void Update(float delta)
    {
        if (AttackFinished && AttackCooldown >= 0)
        {
            AttackCooldown -= delta;
        }
        else if (AttackCooldown <= 0)
        {
            StateMachine.StateChange(this, "GoblinIdle");
        }
    }

    public void OnAnimationFinished()
    {
        GD.Print("Animfinished");
        AttackFinished = true;
    }

    public void PickAnimation(Vector2 dir)
    {
        GD.Print("AnimPiciking");
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
            Anims.Play("AttackDown");
        }
    }
}
