using Godot;
using System;

[Tool]
public partial class DoorLock : StaticBody2D, IInteractable
{
	private Enums.DoorLockType _Type;
	[Export] private Enums.DoorLockType Type
	{
		get => _Type;
		set
		{
			_Type = value;
			UpdateSprite();
		}
	}
	[Export] private Sprite2D Sprite1;
	[Export] private Sprite2D Sprite2;

	public void Interact(Player Player)
	{
		Player.Inventory.AddToItemCount(Enums.ItemType.KEY, -1);
		//play sound
		CallDeferred(MethodName.QueueFree);
	}

	private void UpdateSprite()
	{
		if (Sprite1 is null || Sprite2 is null) return;

		int Frame = (int)Type * 2;

		Sprite1.Frame = Frame;
		Sprite2.Frame = Frame;
	}
}
