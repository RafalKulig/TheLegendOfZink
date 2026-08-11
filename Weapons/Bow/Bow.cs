using Godot;
using System;

public partial class Bow : Node, IWeapon
{
    private string _name;
    private Vector2 dir;
    private PackedScene arrowScene = GD.Load<PackedScene>("res://Weapons/Arrow/arrow.tscn");

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

        Arrow spawnedArrow = arrowScene.Instantiate<Arrow>();

        Player.GetTree().Root.AddChild(spawnedArrow);

        spawnedArrow.GlobalPosition = Player.GlobalPosition;
        spawnedArrow.Rotation = Player.LastDirection.Angle();
        spawnedArrow.Velocity = Player.LastDirection;

        AttackingAnimation(dir);
    }

    public void Exit(Player Player)
    {

    }

    public void AttackingAnimation(Vector2 dir)
    {
        if (dir == Vector2.Left) AnimationName = "BowAttackLeft";
        if (dir == Vector2.Right) AnimationName = "BowAttackRight";
        if (dir == Vector2.Up) AnimationName = "BowAttackUp";
        if (dir == Vector2.Down) AnimationName = "BowAttackDown";
    }
}
