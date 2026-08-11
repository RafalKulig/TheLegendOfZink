using Godot;
using System;

public partial class Arrow : CharacterBody2D
{
    const int SPEED = 150;

    public override void _Ready()
    {
        GD.Print("strzala");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Velocity.Normalized();

        Velocity = direction * SPEED;

        KinematicCollision2D collision = MoveAndCollide(Velocity * (float)delta);

        if (collision != null)
        {
            GD.Print("Jednak gracz");
            QueueFree();
        }
    }

    public void OnLifeTimeOut()
    {
        QueueFree();
    }
}
