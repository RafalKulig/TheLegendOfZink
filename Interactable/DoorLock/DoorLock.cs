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
	[Export] private Sprite2D Sprite;

	public void Interact(Player Player)
	{
		Player.Inventory.AddToItemCount(Enums.ItemType.KEY, -1);
		//play sound
		CallDeferred(MethodName.QueueFree);
	}

	private void UpdateSprite()
	{
		if (Sprite is null) return;

		int Frame = (int)Type;

		Sprite.Frame = Frame;
	}
}
