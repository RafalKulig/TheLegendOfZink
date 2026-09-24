using Godot;
using System;

[Tool]
public partial class Pot : StaticBody2D
{
    [ExportGroup("Type & Visiuals")]
    private Enums.PotType _potType;
    [Export] private Enums.PotType ChestType
    {
        get => _potType;
        set
        {
            _potType = value;
            UpdateSprite();
        }
    }
    [Export] private Sprite2D Sprite;

    [ExportGroup("Reset Settings")]
    [Export] private bool CanReset = false;
    [Export] private float ResetTime = 300;

    [ExportGroup("Components")]
    [Export] private Timer ResetTimer;
    [Export] private Area2D Area2D;
    [Export] private CollisionShape2D CollisionShape;

    private bool IsBroken = false;
    private PackedScene MoneyScene = GD.Load<PackedScene>("res://Items/Collectables/Money/Coin/Coin.tscn");

    public override void _Ready()
    {
        if (ResetTimer is not null)
        {
            ResetTimer.Timeout += OnResetTimerTimeout;
        }

        if (Area2D is not null)
        {
            Area2D.AreaEntered += Interact;
        }

        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (Sprite is null) return;

        int BaseFrame = (int)ChestType * 2;
        Sprite.Frame = IsBroken ? BaseFrame + 1 : BaseFrame;
    }

    private void Interact(Area2D area)
    {
        if (area is Hitbox && area.GetParent() is Player)
        {
            SmashPot();
            SpawnLoot();
        }
    }

    private void SmashPot()
    {
        IsBroken = true;
        UpdateSprite();

        if (CanReset && ResetTimer is not null)
        {
            ResetTimer.Start(ResetTime);
        }

        CollisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }

    private void SpawnLoot()
    {
        float SpawnChance = GD.Randf();
        GD.Print(SpawnChance);
        if (SpawnChance < 0.4f) return;

        Collectable SpawnedCoin = MoneyScene.Instantiate<Collectable>();

        
        GetTree().Root.GetNode("Game").CallDeferred(MethodName.AddChild, SpawnedCoin);

        SpawnedCoin.GlobalPosition = GlobalPosition;

        Vector2 targetPosition = GlobalPosition + new Vector2(0, -15);

        Tween tween = CreateTween();
        tween.TweenProperty(SpawnedCoin, PropertyName.GlobalPosition.ToString(), targetPosition, 1f)
            .SetTrans(Tween.TransitionType.Bounce)
            .SetEase(Tween.EaseType.Out);

        float WhichCoin = GD.Randf();
        if (WhichCoin <= 0.5f)
        {
            //Coin
            GD.Print("Coin");
        }
        else if (WhichCoin > 0.5f && WhichCoin <= 0.85f)
        {
            //SilverCoin
            GD.Print("SilverCoin");
        }
        else if (WhichCoin > 0.85f)
        {
            //Ruby
            GD.Print("Ruby");
        }
    }

    private void OnResetTimerTimeout()
    {
        IsBroken = false;
        UpdateSprite();

        CollisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
    }
}
