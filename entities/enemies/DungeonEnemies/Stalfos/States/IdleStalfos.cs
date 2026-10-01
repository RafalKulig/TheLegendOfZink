using Godot;
using System;

public partial class IdleStalfos : State
{
	[Export] AnimatedSprite2D Anims;
    [Export] Stalfos Enemy;

    public override void Entry()
    {
        Enemy.Velocity = Vector2.Zero;
        Anims.Play("default");
    }

    public override void Update(float delta)
    {
        StateMachine.StateChange(this, "MoveStalfos");
    }
}
