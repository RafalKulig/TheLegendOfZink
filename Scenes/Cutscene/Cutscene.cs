using Godot;
using System;
using System.Threading.Tasks;

public partial class Cutscene : Area2D
{
	[Export] private RichTextLabel CutsceneTextLabel;
    [Export] private string[] DialogueText;
    [Export] private float TextSpeed = 0.1f;
    [Export] private int TextSize = 12;
    private Player Player;

    public override void _Ready()
    {
        for (int i = 0; i < DialogueText.Length; i++)
        {
            CutsceneTextLabel.Text += DialogueText[i] + "\n";
        }
        CutsceneTextLabel.AddThemeFontSizeOverride("normal_font_size", TextSize);
        BodyEntered += OnBodyEntered;
    }

    private async void StartCutscene()
    {
        for (int i = 0; i < CutsceneTextLabel.Text.Length; i++)
        {
            CutsceneTextLabel.VisibleCharacters = i + 1;
            await ToSignal(GetTree().CreateTimer(TextSpeed), "timeout");
        }
        EndCutscene();
    }

    private async void EndCutscene()
    {
        Player.ForceLockState(false);
        CallDeferred(MethodName.QueueFree);
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not Player player) return;
        GD.Print("wszedl");
        Player = player;
        Player.ForceLockState(false);
        Player.ForceLockState(true);

        StartCutscene();
    }
}
