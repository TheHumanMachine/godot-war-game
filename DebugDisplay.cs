using Godot;
using System;

public partial class DebugDisplay : Control
{
	// Called when the node enters the scene tree for the first time.
	Label fpsCounter;
	public override void _Ready()
	{
		fpsCounter = GetNode<Label>("fpscounter");
		fpsCounter.Text = "FPS: ";
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		fpsCounter.Text = "FPS Counter: " + Engine.GetFramesPerSecond();
	}
}
