using Godot;
using System;

[GlobalClass]
public partial class InventorySlot : PanelContainer
{
	[Signal] public delegate void SlotClickedEventHandler(InventorySlot sender, int info);

	[Export] protected TextureRect iconRect;
	[Export] protected Label label;

	public virtual void UpdateSlot(Texture2D texture, int type, string text = "")
	{
		if (texture is not null)
		{
			iconRect.Texture = texture;
			iconRect.Visible = true;
			label.Text = text;
		}
		else
		{
			iconRect.Texture = null;
			label.Text = "";
		}
	}
}
