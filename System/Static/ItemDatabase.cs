using Godot;
using System;
using System.Collections.Generic;

public static class ItemDatabase
{
    private static readonly Dictionary<Enums.ItemType, Texture2D> _textures = new()
    {
        { Enums.ItemType.ARROW, GD.Load<Texture2D>("res:///assets/Legend_of_Zink_Asset_Pack/Menu_Icons/PNG/sprIconMiniArrow.png") },
        { Enums.ItemType.BOMB, GD.Load<Texture2D>("res:///assets/Legend_of_Zink_Asset_Pack/Menu_Icons/PNG/sprIconBomb.png") },
        { Enums.ItemType.COIN, GD.Load<Texture2D>("res://assets/Legend_of_Zink_Asset_Pack/Collectables/PNG/sprGoldCoin.png") }
    };

    public static Texture2D GetTexture(Enums.ItemType type)
    {
        return _textures.GetValueOrDefault(type);
    }
}