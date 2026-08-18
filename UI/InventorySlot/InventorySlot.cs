using Godot;
using System;

public partial class InventorySlot : PanelContainer
{
	[Signal] public delegate void SlotClickedEventHandler(InventorySlot sender, Enums.EquipmentSlot slot);

	[Export] private TextureRect iconRect;
	[Export] private Label label;

	public Enums.UnlockType ItemType;

	public void UpdateSlot(Texture2D texture, Enums.UnlockType type, string text = "")
	{
		if (texture is not null)
		{
			ItemType = type;
			iconRect.Texture = texture;
			iconRect.Visible = true;
			label.Text = text;
		}
		else
		{
			ItemType = type;
			iconRect.Texture = null;
			label.Text = "";
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
