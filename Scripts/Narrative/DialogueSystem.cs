using Godot;
using System;

public partial class DialogueSystem : Control
{
    [Export]
    public Label Title { get; set; }

    [Export]
    public Label TextBox { get; set; }

    public override void _Ready()
    {
        Title ??= GetNode<Label>( "CenterContainer/VBoxContainer/Title" );
        TextBox ??= GetNode<Label>( "CenterContainer/VBoxContainer/PanelContainer/TextBox" );
    }

    public void ShowDialogue( string title, string dialogueKey, float duration )
    {
        ShowDialogueCustom( title, Tr( dialogueKey ), duration );
    }

    public void ShowDialogueCustom( string title, string text, float duration )
    {
            //------------------------------
            // TODO:
            //
            // Add transition effects
            //------------------------------
        Title.Text = title;
        TextBox.Text = text;
        Show();

            // Hide the textbox after 'duration' seconds.
        var tween = CreateTween();
        tween.TweenCallback( Callable.From( Hide ) ).SetDelay( duration );
    }
}
