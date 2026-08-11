using Godot;
using System;

public partial class Shield : Node, IWeapon
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

    public bool CanUse(Player Player)
    {
        return true;
    }  

    public void Use(Player Player)
    {
        dir = Player.LastDirection;
        AttackingAnimation(dir);
    }

    public void Exit(Player Player)
    {
        // Implement exit logic if needed
    }

    public void AttackingAnimation(Vector2 dir)
    {
        if (dir == Vector2.Left) AnimationName = "ShieldLeft";
        if (dir == Vector2.Right) AnimationName = "ShieldRight";
        if (dir == Vector2.Up) AnimationName = "ShieldUp";
        if (dir == Vector2.Down) AnimationName = "ShieldDown";
    }
}
