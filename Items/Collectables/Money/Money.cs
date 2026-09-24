using Godot;
using System;

[Tool]
public partial class Money : Collectable
{
	private Enums.MoneyType _CoinType;
	[Export] public Enums.MoneyType CoinType
	{
		get => _CoinType;
		set
		{
            _CoinType = value;
			Type = Enums.ItemType.MONEY;
			UpdateSprite();
		}
	}
	[Export] private AnimatedSprite2D Sprite;

	private void UpdateSprite()
	{
		if (Sprite is null) return;

		switch(CoinType)
		{
			case Enums.MoneyType.BASICCOIN:
				Sprite.Play("BasicCoin");
				count = 1;
				break;

			case Enums.MoneyType.SILVERCOIN:
				Sprite.Play("SilverCoin");
				count = 5;
				break;

			case Enums.MoneyType.RUBY:
				Sprite.Play("Ruby");
				count = 10;
				break;
		}
	}
}
