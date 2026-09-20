using Godot;
using System;

public partial class Locked : State
{
    [Export] private AnimationPlayer Anims;
    [Export] private Player Player;

    public override void Entry() 
    { 
        Player.Velocity = Vector2.Zero;
        Anims.AnimationFinished += OnAnimationFinished;
    }

    private void OnAnimationFinished(StringName animName)
    {
        StateMachine.StateChange(this, "Idle");
    }

    public override void Exit()
    {
        Anims.AnimationFinished -= OnAnimationFinished;
    }
}
