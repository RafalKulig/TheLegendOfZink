using Godot;
using System;

public partial class Bomb : Hitbox
{
	[Export] AnimationPlayer Anims;

	public override void _Ready()
	{
		Anims.Play("Deafult");
	}
}
