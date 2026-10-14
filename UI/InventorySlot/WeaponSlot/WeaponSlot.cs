using Godot;
using System;

public partial class WeaponSlot : InventorySlot
{
    public Enums.UnlockType WeaponType;

    public override void UpdateSlot(Texture2D texture, int Type, string text = "")
    {
        base.UpdateSlot(texture, Type , text);

        if (texture is not null)
        {
            WeaponType = (Enums.UnlockType)Type;
        }
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton MouseBT && MouseBT.Pressed)
        {
            if (MouseBT.ButtonIndex == MouseButton.Left)
            {
                EmitSignal(SignalName.SlotClicked, this, (int)Enums.EquipmentSlot.SlotA);
            }
            if (MouseBT.ButtonIndex == MouseButton.Right)
            {
                EmitSignal(SignalName.SlotClicked, this, (int)Enums.EquipmentSlot.SlotB);
            }
        }
    }
}
