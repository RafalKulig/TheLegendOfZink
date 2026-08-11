using Godot;
using System;

[GlobalClass]
public abstract partial class State : Node
{
	[Export] public StateMachine StateMachine;

	public virtual void Entry() {}
	public virtual void Exit() {}
	public virtual void Update(float delta) {}
	public virtual void PhysicsUpdate(float delta) {}
}
