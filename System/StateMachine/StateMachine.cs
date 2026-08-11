using Godot;
using Godot.Collections;
using System;
using System.Linq;

[GlobalClass]
public partial class StateMachine : Node
{
	private Dictionary<string, State> States = new ();
    State CurrentState;
    [Export] State InitialState; 

    public override void _Ready()
    {
        foreach (State State in GetChildren().OfType<State>())
        {
            States[State.Name.ToString().ToPascalCase()] = State;
        }

        //debug
        foreach (var (key, value) in States)
        {
            GD.Print(key + " --- " + value);
        }

        if (InitialState != null)
        {
            InitialState.Entry();
            CurrentState = InitialState;
        }
    }

    public override void _Process(double delta)
    {
        CurrentState?.Update((float)delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        CurrentState?.PhysicsUpdate((float)delta);
    }

    public void StateChange(State State, string NextStateName)
    {
        if (State == null || State != CurrentState)
            return;

        State NewState = States[NextStateName.ToPascalCase()];
        if (NewState == null)
            return;

        CurrentState?.Exit();

        NewState.Entry();
        CurrentState = NewState;

        GD.Print(CurrentState.Name);
    }
}
