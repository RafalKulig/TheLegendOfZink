using Godot;
using System;

public partial class Attack : State
{
    [Export] private AnimationPlayer Anims;
    [Export] private Player Player;

    private IWeapon CurrentWeapon;

    public override void Entry()
    {
        CurrentWeapon = Player.Inventory.GetActiveWeapon();
        Player.Velocity = Vector2.Zero;
        CurrentWeapon.Use(Player);
        string AnimName = CurrentWeapon.AnimationName;
        Anims.Play(AnimName);
    }

    public override void Exit()
    {
        CurrentWeapon?.Exit(Player);
    }

    public void OnAnimationFinished()
    {
        StateMachine.StateChange(this, "Idle");
    }
}
