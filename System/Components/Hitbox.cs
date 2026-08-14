using Godot;
using System;

[GlobalClass]
public partial class Hitbox : Area2D
{
	[Export] public int Damage { get; private set; }
}
