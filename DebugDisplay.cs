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
		//authority.Text = "Authority: ?";
	}

	public void SetPlayerSource(Player_Controller pc){
		//will need to be updated to change when auth changes in the network
		this.pc = pc;
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		fpsCounter.Text = "FPS Counter: " + Engine.GetFramesPerSecond();
		if(this.pc == null){
			authority.Text = "Authority: ?";
		}else{
			authority.Text = "Authority: " + this.pc.Name;
		}
	}
}
