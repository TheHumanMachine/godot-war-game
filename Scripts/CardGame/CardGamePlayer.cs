using Godot;
using System;

public partial class CardGamePlayer : Node3D
{
    private float mouseSensitivity;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		if (!IsMultiplayerAuthority())
			return;

		Input.MouseMode = Input.MouseModeEnum.Captured;
		//GetNode<CanvasLayer>("HUDLayer").Visible = true;
		GetNode<Camera3D>("Head/Camera3D").Current = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {

	}

	public override void _UnhandledInput(InputEvent @event) {
		if (!IsMultiplayerAuthority())
			return;


	}

	public override void _PhysicsProcess(double delta)
	{
		
	}
}
