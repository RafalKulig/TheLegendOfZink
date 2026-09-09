using Godot;
using System;

public partial class Slime : Enemy
{
    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }
}
