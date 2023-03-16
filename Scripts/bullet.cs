using Godot;
using System;

public partial class bullet : RigidBody3D
{

	[Export]
	private Player_Controller sourcePlayer;
	[Export]
	private ProjectileWeapon gun;
	[Export]
	private int damage;
	[Export]
	private int speed;

	public bool shoot = false;


	public bullet () {
		
	}

	public void setValues(Player_Controller sourcePlayer, ProjectileWeapon gun, int damage, int speed) {
		this.sourcePlayer = sourcePlayer;
		this.gun = gun;
		this.damage = damage;
		this.speed = speed;
	}


	public override void _Ready()
	{
		this.TopLevel = true;
		
	}

	public override void _Process(double delta)
	{
		
	}

	public override void _PhysicsProcess(double delta) {
		
		GetNode<RayCast3D>("RayCast3D").TargetPosition = new Vector3(0,0,-speed/30);
		GetNode<RayCast3D>("RayCast3D2").TargetPosition = new Vector3(0,0,-speed/30);
		GetNode<RayCast3D>("RayCast3D3").TargetPosition = new Vector3(0,0,-speed/30);
		GetNode<RayCast3D>("RayCast3D4").TargetPosition = new Vector3(0,0,-speed/30);
		GetNode<RayCast3D>("RayCast3D5").TargetPosition = new Vector3(0,0,-speed/30);
	

		if(shoot) {
			ApplyImpulse(-Transform.Basis.Z * speed, Transform.Basis.Z);
			shoot = false;
			GetNode<RayCast3D>("RayCast3D").TargetPosition = new Vector3(0,0,-speed/15);
			GetNode<RayCast3D>("RayCast3D2").TargetPosition = new Vector3(0,0,-speed/15);
			GetNode<RayCast3D>("RayCast3D3").TargetPosition = new Vector3(0,0,-speed/15);
			GetNode<RayCast3D>("RayCast3D4").TargetPosition = new Vector3(0,0,-speed/15);
			GetNode<RayCast3D>("RayCast3D5").TargetPosition = new Vector3(0,0,-speed/15);
		} 

		var hit = GetNode<RayCast3D>("RayCast3D").GetCollider();
		var hit2 = GetNode<RayCast3D>("RayCast3D2").GetCollider();
		var hit3 = GetNode<RayCast3D>("RayCast3D3").GetCollider();
		var hit4 = GetNode<RayCast3D>("RayCast3D4").GetCollider();
		var hit5 = GetNode<RayCast3D>("RayCast3D5").GetCollider();


		if (hit != null && hit.IsClass(nameof(CharacterBody3D))) {
			

			_on_area_3d_body_entered((Node3D)hit);
			
		}
		else if (hit2 != null && hit2.IsClass(nameof(CharacterBody3D))) {
			

			_on_area_3d_body_entered((Node3D)hit2);
			
		}
		else if (hit3 != null && hit3.IsClass(nameof(CharacterBody3D))) {
			

			_on_area_3d_body_entered((Node3D)hit3);
			
		}
		else if (hit4 != null && hit4.IsClass(nameof(CharacterBody3D))) {
			

			_on_area_3d_body_entered((Node3D)hit4);
			
		}
		else if (hit5 != null && hit5.IsClass(nameof(CharacterBody3D))) {
			

			_on_area_3d_body_entered((Node3D)hit5);
			
		}
	}

	private void _on_area_3d_body_entered(Node3D body) {

		if (body.IsClass(nameof(CharacterBody3D))) {

			CharacterBody3D hit_player = (CharacterBody3D)body;

			CharacterBody3D bullet_source_player = (CharacterBody3D)sourcePlayer;

			int peerID = hit_player.GetMultiplayerAuthority();
			int sourceID = bullet_source_player.GetMultiplayerAuthority();

			hit_player.RpcId(peerID, "ReceiveDamage", damage); // this should send a param that tells the client how much damage.

			GD.Print("Bullet hit! | ReceiveDamage request sent by id: " + peerID +  " | Position" + Position);

			QueueFree();
		} else {
			QueueFree();
			GD.Print("Bullet hit something that wasn't a CharacterBody3D | Bullet position: " + Position);
		}
	}
	//on collision, report damage if colliding player. Else fkin die
	private void _on_timer_timeout() {
		QueueFree();
		GD.Print("baleted");
	}
}

