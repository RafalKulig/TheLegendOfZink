using Godot;
using System;

[Tool]
public partial class Camera2d : Camera2D
{
    [Export] private RemoteTransform2D RemoteTransform;
    [Export] private Vector2 DeafultOffset;
    [Export] float DefaultZoom;
    [Export] private Vector4 DeafultLimit;
    [Export] private bool SetDeafult
    {
        get => false;
        set
        {
            if (value)
            {
                SetDefaultCamera();
            }
        }
    }

    public override void _Ready()
    {
        SetDefaultCamera();

        if (Engine.IsEditorHint()) return;

        WorldEvents.Instance.CameraChange += CameraChange;
        WorldEvents.Instance.SetCameraToDefault += SetDefaultCamera;
    }

    private void CameraChange(float _Zoom, Vector4 _Limit, Vector2 _Offset, bool _RemoteTransformOn)
    {
        Offset = _Offset;
        Zoom = new Vector2(_Zoom, _Zoom);
        LimitLeft = (int)_Limit.X;
        LimitTop = (int)_Limit.Y;
        LimitRight = (int)_Limit.Z;
        LimitBottom = (int)_Limit.W;

        if (RemoteTransform is not null)
        {
            RemoteTransform.UpdatePosition = _RemoteTransformOn;
        }
    }

    private void SetDefaultCamera()
    {
        Offset = DeafultOffset;
        Zoom = new Vector2(DefaultZoom, DefaultZoom);
        LimitLeft = (int)DeafultLimit.X;
        LimitTop = (int)DeafultLimit.Y;
        LimitRight = (int)DeafultLimit.Z;
        LimitBottom = (int)DeafultLimit.W;

        if (RemoteTransform is not null)
        {
            RemoteTransform.UpdatePosition = true;
        }
    }
}
