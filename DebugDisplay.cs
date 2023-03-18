using Godot;
using System;

public partial class DebugDisplay : Control
{
	private Player_Controller pc;
	Label fpsCounter;
	Label authority;
	
	public override void _Ready()
	{
		fpsCounter = GetNode<Label>("fpscounter");
		fpsCounter.Text = "FPS: ";
		authority = GetNode<Label>("authority");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		fpsCounter.Text = "FPS Counter: " + Engine.GetFramesPerSecond();
		authority.Text = "Authority: " + GetTree().GetMultiplayer().GetUniqueId();
	}
}
