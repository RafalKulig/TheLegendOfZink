using Godot;
using System;

[Tool]
public partial class ItemToUnlock : Area2D
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

    public enum UnlockType
    {
        BOW,
        BOMB,
        SHIELD,
        WAND,
        BOOMERANG,
    }
    [Export]
    public UnlockType Type { get; set; }

    public void OnBodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            player.ItemUnlock(Type);
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
