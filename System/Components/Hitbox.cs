using Godot;
using System;

[GlobalClass]
public partial class Hitbox : Area2D
{
	[Export] public int Damage { get; private set; }
    [Export] public int KnockbackPower { get; private set; }

	public Vector2 HitDirection { get; set; }
}
