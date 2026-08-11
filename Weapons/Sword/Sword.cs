using Godot;
using System;

public partial class Sword : Node, IWeapon
{
    private string _name;
    private Vector2 dir;

    public string AnimationName
    {
        get => _name;
        set
        {
            _name = value;
        }
    }
	
	public bool CanUse(Player player)
	{
		return true;
	}

    public void Use(Player player)
	{
        if (player.FindChild("SwordHitBox") is Area2D HitBox)
        {
            dir = player.LastDirection;
            if (dir == Vector2.Left) HitBox.Position = new Vector2(-12, 0);
            if (dir == Vector2.Right) HitBox.Position = new Vector2(12, 0);
            if (dir == Vector2.Up) HitBox.Position = new Vector2(0, -10);
            if (dir == Vector2.Down) HitBox.Position = new Vector2(0, 10);

            HitBox.Visible = true;
            HitBox.Monitoring = true;
            AttackingAnimation(dir);
        }
    }

	public void Exit(Player player)
	{
        Area2D HitBox = (Area2D)player.FindChild("SwordHitBox");

        HitBox.Visible = false;
        HitBox.Monitoring = false;
    }

    public void AttackingAnimation(Vector2 dir)
    {
        if (dir == Vector2.Left) AnimationName = "SwordAttackLeft";
        if (dir == Vector2.Right) AnimationName = "SwordAttackRight";
        if (dir == Vector2.Up) AnimationName = "SwordAttackUp";
        if (dir == Vector2.Down) AnimationName = "SwordAttackDown";
    }
}
