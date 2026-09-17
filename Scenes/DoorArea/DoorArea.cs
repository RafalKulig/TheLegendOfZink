using Godot;
using System;

public partial class DoorArea : Area2D
{
	[Export] private Vector2 GoToPosition;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not Player) return;

        //TransitonAnim();

        body.GlobalPosition = GoToPosition;
    }
}
