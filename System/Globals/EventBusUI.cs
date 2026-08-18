using Godot;
using System;

public partial class EventBusUI : Node
{
    public static EventBusUI Instance { get; private set; }

    [Signal] public delegate void UIVisibilityChangedEventHandler(string UiName, bool IsOpen);

    public override void _EnterTree()
    {
        Instance = this;
    }
}