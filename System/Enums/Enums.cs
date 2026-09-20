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
        COIN,
        ARROW,
        BOMB,
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
}
