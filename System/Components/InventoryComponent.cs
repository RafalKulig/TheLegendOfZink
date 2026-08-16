using Godot;
using System;
using System.Collections.Generic;
using static Enums;

[GlobalClass]
public partial class InventoryComponent : Node2D
{
    [Signal] public delegate void ItemCountChangedEventHandler(Enums.ItemType item, int count);
    //[Signal] public delegate void WeaponEquippedEventHandler(Enums.EquipmentSlot slot, IWeapon weapon);
    [Signal] public delegate void WeaponUnlockedEventHandler(Enums.UnlockType type);

    private Dictionary<Enums.ItemType, int> Items = new();
    private Dictionary<Enums.EquipmentSlot, IWeapon> Equiped = new();
    private HashSet<Enums.UnlockType> Unlocks = new();

    public Enums.EquipmentSlot ActiveSlot { get; set; } = Enums.EquipmentSlot.SlotA;

    public override void _Ready()
    {
        Items[Enums.ItemType.COIN] = 0;
        Items[Enums.ItemType.ARROW] = 0;
    }

    public void EquipWeaponToSlot(IWeapon weapon, Enums.EquipmentSlot slot)
    {
        if (weapon is null) return;

        Equiped[slot] = weapon;
    }

    public IWeapon GetActiveWeapon()
    {
        return Equiped[ActiveSlot];
    }

    public void UnlockWeapon(Enums.UnlockType type)
    {
        Unlocks.Add(type);
        GD.Print("odblokowano: " + type.ToString());
    }

    public bool CheckIfUnlocked(Enums.UnlockType itemType)
    {
        return Unlocks.Contains(itemType);
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
    }

    public void ShowEq()
    {
        GD.Print("Items:");
        foreach (var item in Items)
        {
            GD.Print(item.Key + ": " + item.Value);
        }
        GD.Print("------------");
        GD.Print("Equipped Weapons:");
        foreach (var slot in Equiped)
        {
            GD.Print(slot.Key + ": " + slot.Value?.GetType().Name);
        }
        GD.Print("------------");
        GD.Print("Unlocked Weapons:");
        foreach (var weapon in Unlocks)
        {
            GD.Print(weapon);
        }
    }
}
