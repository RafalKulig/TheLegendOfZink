using Godot;
using System;

public partial class SlimeIdle : State
{
	[Export] private Enemy Enemy;
    [Export] private AnimatedSprite2D Anims;

	private Player Player;

    public override void Entry()
    {
        Player = GetTree().Root.GetNode<Player>("/root/Overworld/Player");

        Anims.Play("Idle");
    }

    public override void Update(float delta)
    {
        Vector2 direction = Player.GlobalPosition - Enemy.GlobalPosition;
        if (direction.Length() < 80)
        {
            StateMachine.StateChange(this, "Move");
            return;
        }
    }

}
