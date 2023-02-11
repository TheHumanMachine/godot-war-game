using Godot;
using System;

public partial class bullet : RigidBody3D
{

	[Export]
	private Node sourcePlayer;
	[Export]
	private projectile_weapon gun;
	[Export]
	private int damage;
	[Export]
	private int speed;

	public bool shoot = false;


	public bullet () {
		
	}

	public void setValues(Node sourcePlayer, projectile_weapon gun, int damage, int speed) {
		this.sourcePlayer = sourcePlayer;
		this.gun = gun;
		this.damage = damage;
		this.speed = speed;
		GD.Print("Bullet received shoot from setValues");
	}


	public override void _Ready()
	{
		this.TopLevel = true;
	}

	public override void _Process(double delta)
	{

	}

	public override void _PhysicsProcess(double delta) {
		if(shoot) {
			ApplyImpulse(-Transform.Basis.Z * speed, Transform.Basis.Z);
			shoot = false;
		}
	}

	private void _on_area_3d_body_entered(Node3D body) {
		if (body.IsClass("CharacterBody3D")) {
			CharacterBody3D hit_player = (CharacterBody3D)body;
			int peerID = hit_player.GetMultiplayerAuthority();
			GD.Print("receive damage being sent to: " + peerID);
			hit_player.RpcId(peerID, "ReceiveDamage", damage); // this should send a param that tells the client how much damage.
			GD.Print(Position);
			QueueFree();
			GD.Print("Died because hit player");
		} else {
			GD.Print(Position);
			QueueFree();
			GD.Print("Died because hit something else");
		}
	}

	//on collision, report damage if colliding player. Else fkin die

}

