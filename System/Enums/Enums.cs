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

    }

    public enum UnlockType
    {
        BOW,
        BOMB,
        SHIELD,
        WAND,
        BOOMERANG,
    }
}
