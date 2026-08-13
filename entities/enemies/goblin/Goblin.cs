using Godot;
using System;
using System.Threading.Tasks;

public partial class Goblin : CharacterBody2D
{
    public Vector2 LastDirection { get; private set; }

    public override void _PhysicsProcess(double delta)
    {
        if (Velocity != Vector2.Zero)
        {
            LastDirection = Velocity.Normalized();
        }
        MoveAndSlide();    
    }
}
