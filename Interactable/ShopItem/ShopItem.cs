using Godot;
using System;

[Tool]
public partial class ShopItem : Area2D
{
	[Export] private bool IsRandom = false;
	private int _Price;
	[Export] private int Price
	{
		get => _Price;
		set
		{
			_Price = value;
			Update();
		}
	}
	[Export] private int Count = 1;
	private int CurrentCount;
	private Enums.ItemType _ItemType;
	[Export]
	private Enums.ItemType ItemType
	{
		get => _ItemType;
		set
		{
			_ItemType = value;
			Update();
		}
	}
	[Export] private Sprite2D Sprite;
	[Export] private Label PriceLabel;
	[Export] private Timer Timer;

	private bool PlayerInside;

	private void Update()
	{
		if (Sprite is null || PriceLabel is null) return;

		Sprite.Texture = ItemDatabase.GetTexture(ItemType);
		PriceLabel.Text = Price.ToString() + "$";
	}

    public override void _Ready()
    {
		Reset();

		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
		
		if (Timer is not null)
		{
			Timer.Timeout += OnTimerTimeout;
		}
    }

	private void OnBodyEntered(Node2D Body)
	{
		if (Body is not Player Player) return;

		if (Count == 0) return;

		PlayerInside = true;

		if (Player.Inventory.GetItemCount(Enums.ItemType.MONEY) < _Price)
		{
			return;
		}

		Player.Inventory.AddToItemCount(_ItemType, 1);
		Player.Inventory.AddToItemCount(Enums.ItemType.MONEY, -_Price);
		Count--;

		if (Count == 0)
		{
			Despawn();
		}
	}

	private void OnBodyExited(Node2D Body)
	{
		if (Body is not Player) return;

		PlayerInside = false;
	}

	private void Despawn()
	{
		Timer.Start(3);
        Sprite.Visible = false;
		SetDeferred(Area2D.PropertyName.Monitoring, false);
	}

	private void OnTimerTimeout()
	{
		Reset();
		
		while (PlayerInside)
		{
			continue;
		}

        Sprite.Visible = true;
        SetDeferred(Area2D.PropertyName.Monitoring, true);
    }

	private void Reset()
	{
        if (IsRandom)
        {
            Count = (int)(GD.Randi() % 5 + 1);
            ItemType = (Enums.ItemType)(GD.Randi() % (Enum.GetNames<Enums.ItemType>().Length - 2) + 2);
            Price = ItemType switch
            {
                Enums.ItemType.BOMB => (int)(GD.Randi() % 11 + 15),
                Enums.ItemType.ARROW => (int)(GD.Randi() % 8 + 5),
                Enums.ItemType.HEALTH_POTION => (int)(GD.Randi() % 19 + 20),
            };
        }

        CurrentCount = Count;
        Update();
    }
}
