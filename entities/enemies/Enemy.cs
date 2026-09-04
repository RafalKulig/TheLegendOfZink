using Godot;
using System;

[GlobalClass]
public partial class Enemy : CharacterBody2D
{
    [Signal] public delegate void DiedEventHandler(Enemy enemy);
}
