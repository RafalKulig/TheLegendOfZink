using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using static Enums;

[GlobalClass]
public partial class InventoryComponent : Node2D
{
    [Signal] public delegate void ItemCountChangedEventHandler(Enums.ItemType item, int count);
    //[Signal] public delegate void WeaponEquippedEventHandler(Enums.EquipmentSlot slot, IWeapon weapon);
    [Signal] public delegate void WeaponUnlockedEventHandler(Enums.UnlockType type);
    [Signal] public delegate void InventoryUpdatedEventHandler();

    private Dictionary<Enums.ItemType, int> Items = new();
    private Dictionary<Enums.EquipmentSlot, IWeapon> Equiped = new();
    private HashSet<Enums.UnlockType> Unlocks = new();

    public Enums.EquipmentSlot ActiveSlot { get; set; } = Enums.EquipmentSlot.SlotA;

    public override void _Ready()
    {
        Equiped[Enums.EquipmentSlot.SlotA] = null;
        Equiped[Enums.EquipmentSlot.SlotB] = null;

        Items[Enums.ItemType.COIN] = 0;
        Items[Enums.ItemType.ARROW] = 0;
        Items[Enums.ItemType.BOMB] = 0;
    }

    public Texture2D GetTextureFromWeapon(IWeapon weapon)
    {
        if (weapon == null) return null;

        string path = "res://assets/Legend_of_Zink_Asset_Pack/Menu_Icons/PNG/sprIcon";
        if (weapon is Sword) path += "Sword.png";
        if (weapon is Bow) path += "Bow.png";
        if (weapon is Shield) path += "Shield.png";

        Texture2D texture = ResourceLoader.Load<Texture2D>(path);
        
        return texture;
    }

    public IWeapon GetWeaponFromSlot(Enums.EquipmentSlot slot)
    {
        if(Equiped.TryGetValue(slot, out var weapon))
        {
            return weapon;
        }
        return null;
    }

    public void EquipWeaponToSlot(Enums.UnlockType WeaponType, Enums.EquipmentSlot TargetSlot)
    {
        IWeapon weapon = null;
        switch (WeaponType)
        {
            case Enums.UnlockType.BOW:
                weapon = new Bow();
                break;
            case Enums.UnlockType.SHIELD:
                weapon = new Shield();
                break;
            case Enums.UnlockType.BOOMERANG:
                //weapon = new Boomerang();
                break;
            case Enums.UnlockType.WAND:
                //weapon = new Wand();
                break;
            case Enums.UnlockType.SWORD:
                weapon = new Sword();
                break;
        }

        Enums.EquipmentSlot OtherSlot;
        if (TargetSlot == Enums.EquipmentSlot.SlotA)
        {
            OtherSlot = Enums.EquipmentSlot.SlotB;
        }
        else
        {
            OtherSlot = Enums.EquipmentSlot.SlotA;
        }

        if (Equiped[OtherSlot] is not null && Equiped[OtherSlot].Type == weapon.Type)
        {
            Equiped[OtherSlot] = null;
        }

        Equiped[TargetSlot] = weapon;
    }

    public IWeapon GetActiveWeapon()
    {
        return Equiped[ActiveSlot];
    }

    public void UnlockWeapon(Enums.UnlockType type)
    {
        Unlocks.Add(type);
        GD.Print("odblokowano: " + type.ToString());
        EmitSignal(SignalName.InventoryUpdated);
    }

    public Texture2D GetTextureFromUnlocked(Enums.UnlockType type)
    {
        string path = "res://assets/Legend_of_Zink_Asset_Pack/Menu_Icons/PNG/sprIcon";
        switch (type)
        {
            case Enums.UnlockType.BOW:
                path += "Bow.png";
                break;
            case Enums.UnlockType.SHIELD:
                path += "Shield.png";
                break;
            case Enums.UnlockType.BOOMERANG:
                path += "Boomerang.png";
                break;
            case Enums.UnlockType.WAND:
                path += "Wand.png";
                break;
            case Enums.UnlockType.SWORD:
                path += "Sword.png";
                break;
        }
        Texture2D texture = ResourceLoader.Load<Texture2D>(path);
        return texture;
    }

    public HashSet<Enums.UnlockType> GetUnlocked()
    {
        return Unlocks;
    }

    public bool CanAttack(Enums.EquipmentSlot slot)
    {
        IWeapon weapon;
        if (Equiped.TryGetValue(slot, out weapon) && weapon is not null)
        {
            return weapon.CanUse(GetParent<Player>());
        }
        return false;
    }

    public int GetItemCount(Enums.ItemType item)
    {
        if (Items.TryGetValue(item, out int count))
        {
            return count;
        }
        return 0;
    }

    public void AddToItemCount(Enums.ItemType item, int count)
    {
        if (Items.ContainsKey(item))
        {
            Items[item] += count;
        }
        EmitSignal(SignalName.ItemCountChanged, (int)item, Items[item]);
        EmitSignal(SignalName.InventoryUpdated);
    }
}
