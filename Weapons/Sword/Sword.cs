using Godot;
using System;

public partial class Sword : Node, IWeapon
{
    private string _name;
    private Vector2 dir;

    public Enums.UnlockType Type => Enums.UnlockType.SWORD;
    

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
        dir = player.LastDirection;
        AttackingAnimation(dir);
    }

    public void Exit(Player player)
    {
        return;
    }


    public void AttackingAnimation(Vector2 dir)
    {
        if (dir == Vector2.Left) AnimationName = "SwordAttackLeft";
        if (dir == Vector2.Right) AnimationName = "SwordAttackRight";
        if (dir == Vector2.Up) AnimationName = "SwordAttackUp";
        if (dir == Vector2.Down) AnimationName = "SwordAttackDown";
    }
}
