using Godot;
using System;

[Tool]
public partial class Collectable : Area2D
{
    private Texture2D _sprite;

    [Export]
    public Texture2D Sprite
    {
        get => _sprite;
        set
        {
            _sprite = value;
            UpdateSprite();
        }
    }

    [Export]
    public Enums.ItemType Type { get; private set; }

    public void OnBodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            player.Inventory.AddToItemCount(Type, 1);
            QueueFree();
        }
    }

    private void UpdateSprite()
    {
        if (GetChildCount() > 0 && GetChild(0) is Sprite2D SpriteNode)
        {
            SpriteNode.Texture = _sprite;
        }
    }
}
