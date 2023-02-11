using Godot;
using System;

public partial class Visible_Bullet : RigidBody3D
{
	public bool shoot = false;
	int speed;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.TopLevel = true;
	}

	public void setSpeed(int speed) {

		this.speed = speed;
		GD.Print("visible bullet set speed");
	}
	
	public override void _Process(double delta)
	{

	}

	public override void _PhysicsProcess(double delta)
	{
		if(shoot) {
			ApplyImpulse(-Transform.Basis.Z * speed, Transform.Basis.Z);
			shoot = false;
		}
	}
	
	private void _on_area_3d_body_entered(Node3D body)
	{
		GD.Print("hit something (visible bullet/)");
		QueueFree();
	}
}







