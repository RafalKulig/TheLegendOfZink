using Godot;
using System;

[GlobalClass]
public partial class RoomArea : Area2D
{
	[Export] private Node2D Overworld;
	[Export] private Node2D RoomNode;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;

        ActivateRoom(false);
	}

	private void OnBodyEntered(Node2D body)
    {
        if (body is not Player) return;

        ActivateRoom(true);
    }

    private void OnBodyExited(Node2D body)
    {
        if (body is not Player) return;

        ActivateRoom(false);
    }

    private void ActivateRoom(bool IsActive) 
    {
        if (Overworld is null || RoomNode is null) return;

        Overworld.Visible = !IsActive;
        RoomNode.Visible = IsActive;

        if (IsActive)
        {
            Overworld.ProcessMode = ProcessModeEnum.Disabled;
            RoomNode.ProcessMode = ProcessModeEnum.Inherit;
        } 
        else
        {
            Overworld.ProcessMode = ProcessModeEnum.Inherit;
            RoomNode.ProcessMode = ProcessModeEnum.Disabled;
        }
    }
}
