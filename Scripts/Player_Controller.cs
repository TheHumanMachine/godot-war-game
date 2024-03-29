using Godot;
using System;

public partial class Player_Controller : CharacterBody3D
{
	
	[Signal]
	public delegate void OnHealthChangedEventHandler(int health);

	private Node3D head;
	private AnimationPlayer animPlayer;
	private GpuParticles3D muzzleFlash;
	private RayCast3D raycast;
	private Label3D healthLabel3D;

	private ProgressBar healthBar;
	private ProgressBar healthBarAboveHead;

	private Label3D networkNumber;

	private Label healthLabel;

	public ProjectileWeapon gun;

	[Export]
	private int health = 100;

	private PackedScene bulletScene = (PackedScene)GD.Load("res://Scenes/bullet.tscn");

	public override void _Ready() {
		GetNode<SubViewport>("HealthBar3D/SubViewport");


		healthBar = GetNode<ProgressBar>("HUDLayer/HUD/HealthBar");
		healthBarAboveHead = GetNode<ProgressBar>("HealthBar3D/SubViewport/HealthBar");
		raycast = GetNode<RayCast3D>("Head/Camera3D/RayCast3D");

		healthLabel = GetNode<Label>("HUDLayer/HUD/healthLabel");
		
		head = GetNode<CollisionShape3D>("Head");

		//animPlayer = GetNode<AnimationPlayer>("Head/generic_gun/AnimationPlayer");
		gun = GetNode<ProjectileWeapon>("Head/projectile_weapon");

		healthLabel3D = GetNode<Label3D>("Health");

		networkNumber = GetNode<Label3D>("NetworkNumber");
		
		SetBulletCommand(new BulletCommand(this, gun));
		networkNumber.Text = this.Name;
		healthLabel3D.Text = this.health.ToString();
		
		if (!IsMultiplayerAuthority())
			return;

		Input.MouseMode = Input.MouseModeEnum.Captured;
		GetNode<CanvasLayer>("HUDLayer").Visible = true;
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

		if (Input.IsActionJustPressed("shoot") && gun.CanShoot()) {

			Rpc(nameof(PlayShootEffects));

			if (raycast.IsColliding()) {
				Vector3 hit_thing = raycast.GetCollisionPoint();
				gun.ShootBullet(hit_thing);
			}
		}
	}

	public override void _EnterTree() {
		SetMultiplayerAuthority(int.Parse(this.Name));
	}

	[Rpc(CallLocal = true)]
	private void PlayShootEffects() {
		Vector3 hit_thing = raycast.GetCollisionPoint();
		gun.PlayShootEffects(hit_thing);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	private void ReceiveDamage(int damage) {
		health = health - damage;
		GD.Print("ReceiveDamage: " + damage + " | ID: " + this.GetMultiplayerAuthority() +" | Requested by remote ID: " + Multiplayer.GetRemoteSenderId() + " | current health: " + health);
		

		if (health <= 0) {
			health = 100;
			Position = Vector3.Zero;
		}
		
		healthBar.Value = health;
		
		GD.Print("HEALTHBAR VALUE IS: " + healthBarAboveHead.Value);

		healthLabel.Text = "Health: " + health + "/100";
		

	}

	public long GetPlayerAuthority(){
		return this.GetMultiplayerAuthority();
	}

	public bool IsPlayerMultiplayerAuthority(){
		return IsMultiplayerAuthority();
	}


	public float mouseSensitivity = 0.07f;
	public const float Speed = 10.0f;
	public const float JumpVelocity = 6.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();


	public override void _Process(double delta) {
		healthBarAboveHead.Value = health;
		healthLabel3D.Text = health.ToString();

	}

	public override void _PhysicsProcess(double delta)
	{

		if (!IsMultiplayerAuthority())
			return;

		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor()) {
			velocity.Y -= gravity * (float)delta;
			if(gun.CanShoot())
				gun.Animate("idle");
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
			if(gun.CanShoot())
				gun.Animate("moving");
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
			if(gun.CanShoot())
				gun.Animate("idle");
		}

		Velocity = velocity;


		MoveAndSlide();
	}

	public RayCast3D GetLookRaycast() {
		return raycast;
	}

	//sets bullet behavior for external modification i.e. upgrade cards. 
	//
	public void SetBulletCommand(BulletCommand bc) {
		gun.bulletCommand = bc;
	}
}
