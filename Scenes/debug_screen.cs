using Godot;
using System;

public partial class debug_screen : Node3D
{
	// Called when the node enters the scene tree for the first time.
	Label fpsCounter;
	public override void _Ready()
	{
		fpsCounter = GetNode<Label>("FPS Counter: " + Engine.GetFramesPerSecond());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
