using Godot;
using System;
using System.Threading.Tasks;

public partial class Goblin : CharacterBody2D
{
    [Export] private HealthComponent healthComponent;

    public Vector2 LastDirection { get; private set; }

    public override void _Ready()
    {
        if (healthComponent is not null)
        {
            healthComponent.Died += OnGoblinDied;
            healthComponent.Damaged += OnGoblinDamaged;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Velocity != Vector2.Zero)
        {
            LastDirection = Velocity.Normalized();
        }
        MoveAndSlide();
    }

    private void OnGoblinDamaged(int amount)
    { 
        GD.Print("ale boli " + Name + "'a " + amount + " zostalo: " + healthComponent.currentHealth);
    }

    private void OnGoblinDied()
    {
        GD.Print("umarl: " + Name);
        QueueFree();
    }
}
