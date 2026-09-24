using Godot;
using System;

public partial class Enums : GodotObject
{
    public enum EquipmentSlot
    {
        SlotA, 
        SlotB  
    }

    public enum ItemType
    {
        MONEY,
        ARROW,
        BOMB,
    }

    public enum MoneyType
    {
        BASICCOIN,
        SILVERCOIN,
        RUBY
    }

    public enum UnlockType
    {
        SWORD,
        BOW,
        SHIELD,
        WAND,
        BOOMERANG,
    }

    public enum ChestType
    {
        BLUE,
        BROWN,
        CAVE,
        GREEN,
        PINK,
        PURPLE,
        ROYAL,
    }

    public enum PotType
    {
        BLUE,
        BROWN,
        CAVE,
        FANCY,
        PINK,
        SAND,
    }
}
