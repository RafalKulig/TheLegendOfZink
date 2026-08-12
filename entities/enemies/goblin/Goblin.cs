using Godot;
using System;

public partial class Goblin : CharacterBody2D
{
    public Vector2 LastDirection { get; set; }
    CharacterBody2D player;

    public override void _Process(double delta)
    {
        //player = GetTree().GetFirstNodeInGroup("Player") as CharacterBody2D;
        //GD.Print(GetTree().GetFirstNodeInGroup("Player").Name);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Velocity != Vector2.Zero)
        {
            LastDirection = Velocity.Normalized();
        }
        MoveAndSlide();    
    }
}
