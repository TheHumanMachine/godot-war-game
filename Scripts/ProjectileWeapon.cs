using Godot;
using System;


public partial class ProjectileWeapon : Node3D
{

	AnimationPlayer anim;
	private PackedScene visibleBulletScene = (PackedScene)GD.Load("res://Scenes/Visible_Bullet.tscn");
	Node3D muzzle;

	public int bullet_speed = 400;
	public int bullet_damage = 10;

	public BulletCommand bulletCommand {get; set;}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		muzzle = GetNode<Node3D>("gun_model/muzzle_point");
		anim = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void ShootBullet(Vector3 pos) {
		bulletCommand.Execute(pos);
	}

	public void PlayShootEffects(Vector3 pos) {
		anim.Stop();
		anim.Play("shoot");

		Visible_Bullet vb = (Visible_Bullet)visibleBulletScene.Instantiate();
		muzzle.AddChild(vb);
		vb.LookAt(pos);
		vb.setSpeed(this.bullet_speed);
		vb.shoot = true;

	}

	public bool CanShoot() {
		return anim.CurrentAnimation != "shoot";
	}

	public void Animate(string animation) {
		anim.Play(animation);
	}
}
