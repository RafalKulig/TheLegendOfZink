using Godot;
using System;

[GlobalClass]
public partial class RoomArea : Area2D
{
	[Export] private Node2D Overworld;
	[Export] private Node2D RoomNode;
    [Export] private Vector2 CameraOffset;
    [Export] private float CameraZoom;
    [Export] private Vector4 CameraLimit;
    [Export] private bool RemoteTransformOn;

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
        WorldEvents.Instance.EmitSignal(WorldEvents.SignalName.CameraChange, CameraZoom, CameraLimit, CameraOffset, RemoteTransformOn);
    }

    private void OnBodyExited(Node2D body)
    {
        if (body is not Player) return;

        ActivateRoom(false);
        WorldEvents.Instance.EmitSignal(WorldEvents.SignalName.SetCameraToDefault);
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
