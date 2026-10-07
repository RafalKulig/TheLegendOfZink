using Godot;
using System;

public partial class Fireball : CharacterBody2D
{
    [Export] AnimationPlayer Anims;
    [Export] Hitbox Hitbox;

    private const int SPEED = 165;

    public Vector2 Direction;

    public override void _Ready()
    {
        Hitbox.AreaEntered += OnAreaEntered;

        Anims.Play("default");
    }

    public override void _PhysicsProcess(double delta)
    {
        Velocity = Direction * SPEED;

        KinematicCollision2D collision = MoveAndCollide(Velocity * (float)delta);

        if (collision is not null)
        {
            CallDeferred(MethodName.QueueFree);
        }
    }

    private void OnTimerTimeout()
    {
        CallDeferred(MethodName.QueueFree);
    }

    private void OnAreaEntered(Node2D Node)
    {
        if (Node is Hurtbox Hurtbox && Hurtbox.GetParent() is Player)
        {
            CallDeferred(MethodName.QueueFree);
        }
    }
}
