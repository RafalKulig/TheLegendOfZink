using Godot;
using System;

public interface IWeapon
{
	Enums.UnlockType Type { get; }
	string AnimationName { get; set; }
	bool CanUse(Player player);
	void Use(Player player);
	void Exit(Player player);
}
