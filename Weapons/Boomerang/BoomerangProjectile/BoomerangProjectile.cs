using Godot;
using System;

public partial class BoomerangProjectile : CharacterBody2D
{
    [Signal] public delegate void BoomerangLifeEndEventHandler();

    [Export] public int Speed;
    [Export] public float RotationSpeed;
    [Export] private Sprite2D Sprite;
    [Export] public Hitbox Hitbox;

    public Vector2 SpawnPos;
    public Vector2 GoToPos;
    public Vector2 Direction;

    public override void _Ready()
    {
        GD.Print(GetParent().Name);
    }

    public override void _PhysicsProcess(double delta)
    {
        Velocity = Direction * Speed;

        Sprite.Rotation += (float)delta * RotationSpeed;

        KinematicCollision2D collision = MoveAndCollide(Velocity * (float)delta);

        if (collision is not null || GlobalPosition.DistanceTo(GoToPos) <= 2)
        {
            Direction *= -1;
        }

        if (GlobalPosition.DistanceTo(SpawnPos) <= 2)
        {
            EmitSignal(SignalName.BoomerangLifeEnd);
            CallDeferred(MethodName.QueueFree);
        }
    }

    public void SetEnemyParams(Enemy Enemy)
    {
        GlobalPosition = Enemy.GlobalPosition + Enemy.LastDirection * 10;
        
        Direction = Enemy.LastDirection;

        SpawnPos = Enemy.GlobalPosition;
        GoToPos = Enemy.GlobalPosition + (Enemy.LastDirection * 50);

        CollisionMask = 8;
        Hitbox.CollisionLayer = 512;
        Hitbox.CollisionMask = 64;
    }
}
