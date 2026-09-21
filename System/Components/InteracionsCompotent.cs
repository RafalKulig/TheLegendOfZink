using Godot;
using System;

public partial class InteracionsCompotent : RayCast2D
{
    private Player player;

    public override void _Ready()
    {
        player = GetParent() as Player;
    }

    public override void _PhysicsProcess(double delta)
    {
        Rotation = player.LastDirection.Angle();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if(@event.IsActionPressed("Interact") && !@event.IsEcho())
        {
            TryInteract();
        }
    }

    public void TryInteract()
    {
        if (!IsColliding()) return;

        GodotObject collider = GetCollider();

        //Chest -> IInteractable
        if (collider is Chest interactable) 
        {
            interactable.Interact(player);
        }
    }
}
