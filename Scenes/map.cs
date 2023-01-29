using Godot;
using System;

public partial class map : Node
{
	private PanelContainer mainMenu;
	private LineEdit addressEntry;
	private PackedScene player_scene = (PackedScene)GD.Load("res://Scenes/Player_Controller.tscn");

	private Control hud; 
	private ProgressBar healthbar;

	const int PORT = 9999;
	private ENetMultiplayerPeer enet_peer = new ENetMultiplayerPeer();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hud = GetNode<Control>("CanvasLayer/HUD");
		healthbar = GetNode<ProgressBar>("CanvasLayer/HUD/HealthBar");
		mainMenu = GetNode<PanelContainer>("CanvasLayer/main menu");
		addressEntry = GetNode<LineEdit>("CanvasLayer/main menu/MarginContainer/VBoxContainer/AddressEntry");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void UpdateHealthBar(int healthValue) {
		healthbar.Value = healthValue;
	}

	private void _on_host_button_pressed()
	{
		hud.Visible = true;
		mainMenu.Visible = false;
		enet_peer.CreateServer(PORT);
		Multiplayer.MultiplayerPeer = enet_peer;
		Multiplayer.PeerConnected += AddPlayer;

		AddPlayer(Multiplayer.GetUniqueId());
	}

	private void _on_join_button_pressed()
	{
		hud.Visible = true;
		mainMenu.Visible = false;
		enet_peer.CreateClient("localhost", PORT);
		Multiplayer.MultiplayerPeer = enet_peer;
	}

	private void AddPlayer(long peerID) {
		Player_Controller player = (Player_Controller)player_scene.Instantiate();
		player.Name = peerID.ToString();
		AddChild(player);

		if (player.IsMultiplayerAuthority()) {
			player.HealthSignal += UpdateHealthBar;
		}
	}

	private void _on_multiplayer_spawner_spawned(Node node) {
		Player_Controller pc = (Player_Controller) node;
		if (pc.IsMultiplayerAuthority()) {
			pc.HealthSignal += UpdateHealthBar;
		}
	}

}
