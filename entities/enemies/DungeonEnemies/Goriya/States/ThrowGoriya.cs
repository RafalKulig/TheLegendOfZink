using Godot;
using System;

public partial class ThrowGoriya : State
{
    [Export] private Goriya Enemy;
    private bool HasThrown = false;

    public override void Entry()
    {
        Enemy.Velocity = Vector2.Zero;
        HasThrown = false;
    }

    public override void Update(float delta)
    {
        if (HasThrown) return;

        BoomerangProjectile SpawnedProjectile = Enemy.ProjectileScene.Instantiate<BoomerangProjectile>();

        GetParent().GetParent().GetParent().AddChild(SpawnedProjectile);

        SpawnedProjectile.BoomerangLifeEnd += StateChange;

        SpawnedProjectile.SetEnemyParams(Enemy);

        HasThrown = true;
    }

    private void StateChange()
    {
        StateMachine.StateChange(this, "MoveGoriya");
    }

    public override void Exit()
    {
        HasThrown = false;
    }
}
