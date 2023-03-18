using Godot;
using System;

public partial class CardGamePlayer : Node3D
{
    private float mouseSensitivity = 0.07f;
	private Node3D head;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		GD.Print("position from node: " + Position);
		if (!IsMultiplayerAuthority())
			return;

		head = GetNode<Node3D>("Head");
		Input.MouseMode = Input.MouseModeEnum.Captured;
		GetNode<Camera3D>("Head/Camera3D").Current = true;

		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {

	}

	public override void _EnterTree() {
		SetMultiplayerAuthority(int.Parse(this.Name));
	}

	public override void _UnhandledInput(InputEvent @event) {
		if (!IsMultiplayerAuthority()) {
			return;
		}

			

		if (@event is InputEventMouseMotion mouse) {
			RotateY((float)(Math.PI / 180.0 * (-mouse.Relative.X * mouseSensitivity)));
			head.RotateX((float)(Math.PI / 180.0 * (-mouse.Relative.Y * mouseSensitivity)));
			head.Rotation = new Vector3((Math.Clamp(head.Rotation.X, (float)(Math.PI / 180.0 * -89),(float)(Math.PI / 180.0 * 89))), head.Rotation.Y, head.Rotation.Z);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		
	}
}
