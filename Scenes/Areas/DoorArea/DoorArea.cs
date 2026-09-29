using Godot;
using System;

[GlobalClass]
public partial class DoorArea : Area2D
{
    [Export] private DoorArea NextRoom;
    [Export] public Marker2D EnterPoint { get; private set; }

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    protected void OnBodyEntered(Node2D body)
    {
        if (body is not Player) return;

        //TransitonAnim();

        body.GlobalPosition = NextRoom.EnterPoint.GlobalPosition;
    }
}
