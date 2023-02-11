using Godot;
using System;


public partial class projectile_weapon : Node3D
{

	AnimationPlayer anim;
	GpuParticles3D emitter;

	public BulletCommand bulletCommand {get; set;}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		anim = GetNode<AnimationPlayer>("AnimationPlayer");
		emitter = GetNode<GpuParticles3D>("VisibleBulletEmitter");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ShootBullet(Vector3 pos) {
		GD.Print("Gun received shoot on authority " + this.GetMultiplayerAuthority());
		bulletCommand.Execute(pos);
	}

	public void PlayShootEffects(Vector3 pos) {
		anim.Stop();
		anim.Play("shoot");
		emitter.LookAt(pos);
		emitter.EmitParticle(GetNode<Node3D>("muzzle_point").Transform, new Vector3(0,0,1), new Color(0,0,0), new Color(0,0,0), (uint)GpuParticles3D.EmitFlags.Velocity);
	}

	public bool CanShoot() {
		return anim.CurrentAnimation != "shoot";
	}

	public void Animate(string animation) {
		anim.Play(animation);
	}

}
