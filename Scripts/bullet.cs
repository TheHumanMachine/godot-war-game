using Godot;
using System;

public partial class bullet : RigidBody3D
{
	private Node sourcePlayer;
	private projectile_weapon gun;

	private int damage;
	private int speed;

	public bool shoot = false;

	public bullet (Node sourcePlayer, projectile_weapon gun, int damage, int speed) {
		this.sourcePlayer = sourcePlayer;
		this.gun = gun;
		this.damage = damage;
		this.speed = speed;
		GD.Print("Bullet received shoot");
	}

	public override void _Ready()
	{
		this.TopLevel = true;
	}

	public override void _Process(double delta)
	{
		//GD.Print(GlobalPosition.ToString());

	}

	public override void _PhysicsProcess(double delta) {
		if(shoot) {
			//ApplyImpulse(-Transform.Basis.Z * speed, Transform.Basis.Z);
		}
	}

	public void _on_area_3d_body_entered(Node n) {
		if (n.IsClass("CharacterBody3D")) {
			CharacterBody3D hit_player = (CharacterBody3D)n;
			int peerID = hit_player.GetMultiplayerAuthority();
			GD.Print("receive damage being sent to: " + peerID);
			hit_player.RpcId(peerID, "ReceiveDamage", damage); // this should send a param that tells the client how much damage.
			QueueFree();
			GD.Print("Died because hit player");
		} else {
			QueueFree();
			GD.Print("Died because hit something else");
		}
	}

	//on collision, report damage if colliding player. Else fkin die

}
