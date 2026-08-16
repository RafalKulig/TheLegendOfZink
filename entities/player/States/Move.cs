using Godot;
using System;

public partial class Move : State
{
	[Export] private Player Player;
    [Export] private AnimatedSprite2D Anims;
    [Export] private float SPEED; 

	private Vector2 MoveDirection = Vector2.Zero;

    public override void Entry()
    {
        MoveDirection = GetCurrentInputDirection();
    }

    public override void Update(float delta)
    {
        if(!IsDirectionStillPressed(MoveDirection))
            MoveDirection = GetCurrentInputDirection();

        if (MoveDirection == Vector2.Zero)
            StateMachine.StateChange(this, "Idle");

        if (Input.IsActionJustPressed("AttackA") && Player.Inventory.CanAttack(Enums.EquipmentSlot.SlotA))
        {
            Player.Inventory.ActiveSlot = Enums.EquipmentSlot.SlotA;
            StateMachine.StateChange(this, "Attack");
            return;
        }

        if (Input.IsActionJustPressed("AttackB") && Player.Inventory.CanAttack(Enums.EquipmentSlot.SlotB))
        {
            Player.Inventory.ActiveSlot = Enums.EquipmentSlot.SlotB;
            StateMachine.StateChange(this, "Attack");
            return;
        }

        WalkingAnimation(MoveDirection);
    }

    public override void PhysicsUpdate(float delta)
    {
        Player.Velocity = MoveDirection * SPEED;
    }

    private Vector2 GetCurrentInputDirection()
    {
        if (Input.IsActionPressed("MoveLeft")) return Vector2.Left;
        if (Input.IsActionPressed("MoveRight")) return Vector2.Right;
        if (Input.IsActionPressed("MoveUp")) return Vector2.Up;
        if (Input.IsActionPressed("MoveDown")) return Vector2.Down;

        return Vector2.Zero;
    }

    private bool IsDirectionStillPressed(Vector2 dir)
    {
        if (dir == Vector2.Left && Input.IsActionPressed("MoveLeft")) return true;
        if (dir == Vector2.Right && Input.IsActionPressed("MoveRight")) return true;
        if (dir == Vector2.Up && Input.IsActionPressed("MoveUp")) return true;
        if (dir == Vector2.Down && Input.IsActionPressed("MoveDown")) return true;

        return false;
    }

    private void WalkingAnimation(Vector2 dir)
    {
        if (dir == Vector2.Left)
            Anims.Play("WalkA");
        if (dir == Vector2.Right)
            Anims.Play("WalkD");
        if (dir == Vector2.Up)
            Anims.Play("WalkW");
        if (dir == Vector2.Down)
            Anims.Play("WalkS");
    }
}
