using Godot;
using System;

public partial class Arrow : CharacterBody2D
{
    [Export] private Hitbox Hitbox;

    const int SPEED = 150;

    public override void _Ready()
    {
        Hitbox.HitDirection = Velocity;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Velocity.Normalized();

        Hitbox.HitDirection = direction;

        Velocity = direction * SPEED;

        KinematicCollision2D collision = MoveAndCollide(Velocity * (float)delta);

        if (collision != null)
        {
            QueueFree();
        }
    }

    public void OnLifeTimeOut()
    {
        QueueFree();
    }
}
