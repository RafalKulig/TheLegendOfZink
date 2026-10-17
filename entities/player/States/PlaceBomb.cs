using Godot;
using System;

public partial class PlaceBomb : State
{
    [Export] private Player Player;
    [Export] private AnimationPlayer AttackAnims;
    [Export] private PackedScene BombScene;

    private Vector2 SpawnPosition;

    public override void _Ready()
    {
        AttackAnims.AnimationFinished += OnAnimationFinished;
    }

    public override void Entry()
    {
        if (Player.Inventory.GetItemCount(Enums.ItemType.BOMB) <= 0) return;

        Player.Velocity = Vector2.Zero;

        SpawnPosition = Player.GlobalPosition;

        Player.Inventory.AddToItemCount(Enums.ItemType.BOMB, -1);

        string WhichAnim = (Player.LastDirection.X, Player.LastDirection.Y) switch
        {
            (-1, 0) => "BombLeft",
            (0, -1) => "BombLeft",
            (1, 0) => "BombRight",
            (0, 1) => "BombRight",
            _ => "BombLeft"
        };

        AttackAnims.Play(WhichAnim);
    }

    public override void Update(float delta)
    {
        if (Player.Inventory.GetItemCount(Enums.ItemType.BOMB) <= 0 && !AttackAnims.IsPlaying())
        {
            StateMachine.StateChange(this, "Idle");
        }
    }

    private void OnAnimationFinished(StringName StringName)
    {
        StateMachine.StateChange(this, "Idle");
    }

    public void SpawnBomb()
    {
        Bomb SpawnedBomb = BombScene.Instantiate<Bomb>();

        GetTree().CurrentScene.AddChild(SpawnedBomb);

        SpawnedBomb.GlobalPosition = SpawnPosition;
    }
}
