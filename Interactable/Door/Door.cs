using Godot;
using System;

public partial class Door : Area2D, IInteractable
{
    [Export] private Door NextRoom;
    [Export] public Marker2D EnterPoint { get; private set; }
    
    public void Interact(Player Player)
	{
        Player.GlobalPosition = NextRoom.EnterPoint.GlobalPosition;
	}
}
