using Godot;
using System;

public partial class Player_Controller : CharacterBody3D
{
	
	[Signal]
	public delegate void HealthSignalEventHandler(int health);

	private Node3D head;
	private AnimationPlayer animPlayer;
	private GpuParticles3D muzzleFlash;
	private RayCast3D raycast;
	private Label3D healthLabel;

	private Label3D networkNumber;

	private int health = 100;

	public override void _Ready() {
		raycast = GetNode<RayCast3D>("Head/Camera3D/RayCast3D");
		head = GetNode<CollisionShape3D>("Head");
		animPlayer = GetNode<AnimationPlayer>("Head/gun/AnimationPlayer");
		healthLabel = GetNode<Label3D>("Health");
		networkNumber = GetNode<Label3D>("NetworkAuthority");

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
			GD.Print("I FIRED: " + this.GetMultiplayerAuthority());
			if (raycast.IsColliding()) {
				GodotObject hit_thing = raycast.GetCollider();
				if(hit_thing.IsClass("CharacterBody3D")) {
					CharacterBody3D hit_player = (CharacterBody3D)hit_thing;
					int peerID = hit_player.GetMultiplayerAuthority();
					GD.Print("receive damage being sent to: " + peerID);
					RpcId(peerID, "ReceiveDamage");
					
				}
			}
		}
	}

	public override void _EnterTree() {
		SetMultiplayerAuthority(int.Parse(this.Name));
		networkNumber.Text = this.Name;
	}


	[Rpc(CallLocal = true)]
	private void PlayShootEffects() {
		animPlayer.Stop();
		animPlayer.Play("shoot");
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	private void ReceiveDamage() {
		health = health - 35;
		GD.Print("I have received damage: " + this.GetMultiplayerAuthority() + " I have " + health + " Health");
		healthLabel.Text = health.ToString();
		Position = Vector3.Zero;
		EmitSignal(SignalName.HealthSignal, health);
		if (health <= 0) {
			health = 100;
		}
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
