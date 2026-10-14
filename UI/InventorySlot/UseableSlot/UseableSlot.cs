using Godot;
using System;

public partial class UseableSlot : InventorySlot
{
    public Enums.ItemType Type;

    public override void UpdateSlot(Texture2D texture, int Type, string text = "")
    {
        base.UpdateSlot(texture, Type, text);
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton MouseBT && MouseBT.Pressed)
        {
            if (MouseBT.ButtonIndex == MouseButton.Left)
            {
                EmitSignal(SignalName.SlotClicked, this, (int)Type);
            }
        }
    }
}
