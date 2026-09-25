using Godot;
using System;

[GlobalClass]
public partial class DungeonRoom : Area2D
{
    [Export] private CollisionShape2D RoomShape;
    private float Zoom = 1.6f;
    private Vector2 Offset = new Vector2(0, -15);
    private bool RemoteTransform = false;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not Player) return;

        Rect2 RoomBounds = GetRoomBounds();
        Vector4 Coords = ConvertBoundsToCoords(RoomBounds);

        WorldEvents.Instance.EmitSignal(WorldEvents.SignalName.CameraChange, Zoom, Coords, Offset, RemoteTransform);
    }

    private Rect2 GetRoomBounds()
    {
        if (RoomShape?.Shape is RectangleShape2D rect)
        {
            Vector2 Size = rect.Size;
            Vector2 Position = RoomShape.GlobalPosition - (Size / 2);
            
            return new Rect2(Position, Size);
        }

        return new Rect2();
    }

    private Vector4 ConvertBoundsToCoords(Rect2 Bounds)
    {
        float X = Bounds.Position.X;
        float Y = Bounds.Position.Y;
        float Z = X + Bounds.Size.X;
        float W = Y + Bounds.Size.Y;

        return new Vector4(X, Y, Z, W);
    }
}
