using Godot;
using System;

public partial class IdleGel : State
{
	[Export] private Timer Timer;
	[Export] private Gel Enemy;

    public override void Entry()
    {
        Enemy.Velocity = Vector2.Zero;
        Timer.Timeout += OnTimerTimeout;
        RandomizeIdleTime();
    }

    private void OnTimerTimeout()
    {
        StateMachine.StateChange(this, "MoveGel");
    }

    private void RandomizeIdleTime()
    {
        if (Timer is null) return;

        double Time = GD.Randfn(0.4, 0.3);

        if (Time > 0) Timer.WaitTime = Time;

        Timer.Start();
    }

    public override void Exit()
    {
        Timer.Stop();
        Timer.Timeout -= OnTimerTimeout; 
    }
}
