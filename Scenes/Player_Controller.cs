using Godot;
using System;

public partial class Player_Controller : CharacterBody3D
{

	private Node3D head;
	private AnimationPlayer animPlayer;
	private GpuParticles3D muzzleFlash;
	private RayCast3D raycast;

	int health = 100;

	public override void _Ready() {
		raycast = GetNode<RayCast3D>("Head/Camera3D/RayCast3D");
		head = GetNode<CollisionShape3D>("Head");
		animPlayer = GetNode<AnimationPlayer>("Head/gun/AnimationPlayer");

		if (!IsMultiplayerAuthority())
			return;


		Input.MouseMode = Input.MouseModeEnum.Captured;
		GetNode<Camera3D>("Head/Camera3D").Current = true;
	}
	
	public override void _UnhandledInput(InputEvent @event) {
		if (!IsMultiplayerAuthority())
			return;


		if (@event is InputEventMouseMotion mouse) {
			RotateY((float)(Math.PI / 180.0 * (-mouse.Relative.X * mouseSensitivity)));
			head.RotateX((float)(Math.PI / 180.0 * (-mouse.Relative.Y * mouseSensitivity)));
			head.Rotation = new Vector3((Math.Clamp(head.Rotation.X, (float)(Math.PI / 180.0 * -89),(float)(Math.PI / 180.0 * 89))), head.Rotation.Y, head.Rotation.Z);

		}

		if (Input.IsActionJustPressed("shoot") && animPlayer.CurrentAnimation != "shoot") {
			Rpc(nameof(PlayShootEffects));
			if (raycast.IsColliding()) {
				GodotObject hit_thing = raycast.GetCollider();
				if(hit_thing.IsClass("CharacterBody3D") && !hit_thing.IsClass("CSGMesh3D")) {
					CharacterBody3D hit_player = (CharacterBody3D)hit_thing;
					RpcId((hit_player.GetMultiplayerAuthority()), "ReceiveDamage");
				}
			}
		}
	}

	public override void _EnterTree() {
		SetMultiplayerAuthority(int.Parse(this.Name));
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	private void ReceiveDamage() {
		health -= 35;
		if (health <= 0) {
			health = 3;
			GlobalPosition = Vector3.Zero;
		}
	}


	[Rpc(CallLocal = true)]
	private void PlayShootEffects() {
		animPlayer.Stop();
		animPlayer.Play("shoot");
	}

	public float mouseSensitivity = 0.07f;
	public const float Speed = 10.0f;
	public const float JumpVelocity = 6.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{

		if (!IsMultiplayerAuthority())
			return;

		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor()) {
			velocity.Y -= gravity * (float)delta;
			if(animPlayer.CurrentAnimation != "shoot")
				animPlayer.Play("idle");
		}
			

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
			if(animPlayer.CurrentAnimation != "shoot")
				animPlayer.Play("moving");
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
			if(animPlayer.CurrentAnimation != "shoot")
				animPlayer.Play("idle");
		}

		Velocity = velocity;


		MoveAndSlide();
	}
}
