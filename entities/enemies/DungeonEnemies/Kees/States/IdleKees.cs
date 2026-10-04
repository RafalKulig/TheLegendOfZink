using Godot;
using System;

public partial class IdleKees : State
{
	[Export] private Kees Enemy;
	[Export] private AnimatedSprite2D Anims;
	[Export] private Timer Timer;

    public override void Entry()
    {
        Enemy.Velocity = Vector2.Zero;
        Timer.Timeout += OnTimerTimeout;
        Anims.Frame = 0;
        Anims.Stop();
        RandomizeIdleTime();
    }

    public override void Exit()
    {
        Timer.Timeout -= OnTimerTimeout;
        Timer.Stop();
    }

    private void OnTimerTimeout()
    {
        StateMachine.StateChange(this, "MoveKees");
    }

    private void RandomizeIdleTime()
    {
        if (Timer is null) return;

        double Time = GD.Randfn(0.75, 0.25);

        if (Time > 0) Timer.WaitTime = Time;

        Timer.Start();
    }
}
