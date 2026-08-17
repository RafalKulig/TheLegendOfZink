using Godot;
using System;

public partial class InventorySlot : PanelContainer
{
	[Export] private TextureRect iconRect;
	[Export] private Label label;

	public void UpdateSlot(Texture2D texture, string text = "")
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
