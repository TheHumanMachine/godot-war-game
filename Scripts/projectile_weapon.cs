using Godot;
using System;


public partial class projectile_weapon : Node3D
{

	AnimationPlayer anim;

	public BulletCommand bulletCommand {get; set;}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		anim = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ShootBullet(Vector3 pos) {
		GD.Print("Gun received shoot on authority " + this.GetMultiplayerAuthority());
		bulletCommand.Execute(pos);
	}

	public void PlayShootEffects() {
		anim.Stop();
		anim.Play("shoot");
	}

	public bool CanShoot() {
		return anim.CurrentAnimation != "shoot";
	}

	public void Animate(string animation) {
		anim.Play(animation);
	}

}
