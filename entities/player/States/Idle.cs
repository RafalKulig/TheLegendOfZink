using Godot;
using System;

public partial class Idle : State
{
    private Vector2 PlayerInput;
    [Export] private Player Player;
    [Export] private AnimatedSprite2D Anims;

    public override void Entry()
    {
        Vector2 dir = Player.LastDirection;
        if (dir == Vector2.Left) Anims.Play("WalkA");
        if (dir == Vector2.Right) Anims.Play("WalkD");
        if (dir == Vector2.Up) Anims.Play("WalkW");
        if (dir == Vector2.Down) Anims.Play("WalkS");
        Anims.Stop();
        Anims.Frame = 1;
        Player.Velocity = Vector2.Zero;
    }

    public override void Update(float delta)
    {
        PlayerInput = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");

        if (PlayerInput != Vector2.Zero)
        {
            StateMachine.StateChange(this, "Move");
            return;
        }

        if (Input.IsActionJustPressed("AttackA") && Player.CanAttack(Enums.EquipmentSlot.SlotA))
        {
            Player.ActiveSlot = Enums.EquipmentSlot.SlotA;
            StateMachine.StateChange(this, "Attack");
            return;
        }

        if (Input.IsActionJustPressed("AttackB") && Player.CanAttack(Enums.EquipmentSlot.SlotB))
        {
            Player.ActiveSlot = Enums.EquipmentSlot.SlotB;
            StateMachine.StateChange(this, "Attack");
            return;
        }
    }
}
