using Godot;
using System;
using Godot.Collections;

public partial class Shield : Node, IWeapon
{
    private Enums.UnlockType _type;
    private string _name;
    private Vector2 dir;

    public Enums.UnlockType Type
    {
        get => _type;
        set
        {
            _type = Enums.UnlockType.SHIELD;
        }
    }

    public string AnimationName
    {
        get => _name;
        set
        {
            _name = value;
        }
    }
    AnimationPlayer AttackAnims;
    private bool IsOnCooldown;

    public bool CanUse(Player Player)
    {
        if (IsOnCooldown) return false;
        return true;
    }  

    public void Use(Player Player)
    {
        Array<Node> PlayerChildren = Player.GetChildren();

        for (int i = 0; i < PlayerChildren.Count; i++)
        {
            if (PlayerChildren[i] is AnimationPlayer Anims && Anims.Name == "AttackAnimPlayer")
            {
                AttackAnims = Anims;
            }
        }

        AttackAnims.AnimationFinished += OnCooldownTimeout;

        dir = Player.LastDirection;
        AttackingAnimation(dir);
        IsOnCooldown = true;
    }

    public void Exit(Player Player)
    {
        AttackAnims.AnimationFinished -= OnCooldownTimeout;
    }

    public void AttackingAnimation(Vector2 dir)
    {
        if (dir == Vector2.Left) AnimationName = "ShieldLeft";
        if (dir == Vector2.Right) AnimationName = "ShieldRight";
        if (dir == Vector2.Up) AnimationName = "ShieldUp";
        if (dir == Vector2.Down) AnimationName = "ShieldDown";
    }

    private void OnCooldownTimeout(StringName AnimName)
    {
        IsOnCooldown = false;
    }
}
