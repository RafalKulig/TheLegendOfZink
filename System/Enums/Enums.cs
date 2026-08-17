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
        BOW,
        SHIELD,
        WAND,
        BOOMERANG,
    }
}
