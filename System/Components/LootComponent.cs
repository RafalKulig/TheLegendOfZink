using Godot;
using System;
using Godot.Collections;

[GlobalClass]
public partial class LootComponent : Node2D
{
	[Export] private Array<PackedScene> PossibleDrops = new();
	[Export] public Array<Enums.ItemType> ExcludedTypes = new();
	[Export(PropertyHint.Range, "0,1,0.05")] public float DropChance { get; private set; } = 1.0f;

    private int ItemTypeCount;

    public override void _Ready()
    {
		ItemTypeCount = Enum.GetNames<Enums.ItemType>().Length + 1;
    }

	public void SpawnRandomLoot()
	{
		if (PossibleDrops is null || PossibleDrops.Count == 0) return;
		if (GD.Randf() > DropChance) return;

		uint RandomIndex = GD.Randi() % (uint)PossibleDrops.Count;
		PackedScene SelectedItem = PossibleDrops[(int)RandomIndex];

		if (SelectedItem is null) return;

		Collectable LootInstance;

		do
		{
			LootInstance = SelectedItem.Instantiate<Collectable>();
		} while (ExcludedTypes.Contains(LootInstance.Type));

		LootInstance.GlobalPosition = GlobalPosition;

		GetTree().CurrentScene.CallDeferred(MethodName.AddChild, LootInstance);
	}

	public Enums.ItemType GetRandomItemType()
	{
		if(ExcludedTypes is null || ExcludedTypes.Count == 0) return (Enums.ItemType)(GD.Randi() % (uint)ItemTypeCount);

        Enums.ItemType RandomType;

        do
        {
            RandomType = (Enums.ItemType)(GD.Randi() % (uint)ItemTypeCount);
        } while (ExcludedTypes.Contains(RandomType));

        return RandomType;
    }

	public int GetRandomItemCount(int MinValue, int MaxValue)
	{
		return (int)(GD.Randi() % (MaxValue + 1 - MinValue) + MinValue);
	}
}