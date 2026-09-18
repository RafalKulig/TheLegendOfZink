using Godot;
using System;

public partial class WorldEvents : Node
{
    public static WorldEvents Instance { get; private set; }
    [Signal] public delegate void CameraChangeEventHandler(float Zoom, Vector4 Limit, Vector2 Offset, bool RemoteTransformOn);
    [Signal] public delegate void SetCameraToDefaultEventHandler();

    public override void _EnterTree()
    {
        Instance = this;
    }
}
