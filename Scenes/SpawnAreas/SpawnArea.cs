using Godot;
using System;
using System.Diagnostics.Tracing;

public partial class SpawnArea : Area2D
{
	[Export] private PackedScene EntityToSpawn;
	[Export] private int MaxEntitySpawned;
	[Export] private double TimeToReset;
	[Export] private bool IsAbleToReset;
    [Export] private CollisionShape2D AreaShape;
	[Export] private ShapeCast2D SpawnChecker;

	private bool IsActive;
	private int CurrentlySpawned;
	private double Timer;

    public override void _Ready()
    {
		this.BodyEntered += OnBodyEntered;
		IsActive = true;
    }

    public override void _Process(double delta)
    {
		if (!IsAbleToReset || IsActive) return;

		Timer += delta;
		if (Timer >= TimeToReset)
		{
			Timer = 0;
			IsActive = true;
		}
    }

    private async void OnBodyEntered(Node2D body)
	{
		if (body is not Player || !IsActive || CurrentlySpawned > 0) return;

		for (int i = 0; i < MaxEntitySpawned; i++)
		{
			SpawnEntity();
            await ToSignal(GetTree().CreateTimer(0.25), "timeout");
        }
	}

	private void SpawnEntity()
	{
		Enemy entity = EntityToSpawn.Instantiate<Enemy>();

		CallDeferred(MethodName.AddChild, entity);

		entity.Died += OnEntityDied;

		entity.Position = GetRandPos();

		CurrentlySpawned++;
	}

	private Vector2 GetRandPos()
	{
		if (AreaShape is null || AreaShape.Shape is null) return Vector2.Zero;

		Rect2 rect = AreaShape.Shape.GetRect();

		Vector2 pos;

        do
        {
            float x = (float)GD.RandRange(rect.Position.X, rect.End.X);
            float y = (float)GD.RandRange(rect.Position.Y, rect.End.Y);

			pos = new Vector2(x, y);
        }
        while (!CanBePlaced(pos));

		return pos;
	}

	private bool CanBePlaced(Vector2 pos)
	{
		if (SpawnChecker is null) return false;

		SpawnChecker.Position = pos;

		SpawnChecker.ForceShapecastUpdate();

		return !SpawnChecker.IsColliding();
	}

	private void OnEntityDied(Enemy enemy)
	{
		enemy.Died -= OnEntityDied;

		CurrentlySpawned--;

		if (CurrentlySpawned == 0)
		{
			IsActive = false;
		}
	}
}
