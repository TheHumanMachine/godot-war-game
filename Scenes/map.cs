using Godot;
using System;
using System.Diagnostics;

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
		Multiplayer.PeerDisconnected += RemovePlayer;

		AddPlayer(Multiplayer.GetUniqueId());

		upnpSetup();
	}

	private void _on_join_button_pressed()
	{
		hud.Visible = true;
		mainMenu.Visible = false;
		enet_peer.CreateClient(addressEntry.Text, PORT);
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

	private void RemovePlayer(long peerID) {
		var player = GetNodeOrNull(peerID.ToString());
		if (player != null) {
			player.QueueFree();
		}
	}

	private void _on_multiplayer_spawner_spawned(Node node) {
		Player_Controller pc = (Player_Controller) node;
		if (pc.IsMultiplayerAuthority()) {
			pc.HealthSignal += UpdateHealthBar;
		}
	}

	private void upnpSetup() {

		GD.Print("upnp func-ass" );

		var upnp = new Upnp();

		var discoverResult = upnp.Discover();

		//Debug.Assert(discoverResult == (int)Upnp.UpnpResult.Success, "UPNP discover failed; error " + discoverResult);

		/*
		if (upnp.GetGateway() == null) {
			GD.Print("Null Gateway");

		}

		if(!upnp.GetGateway().IsValidGateway()) {
			GD.Print("invalid Gateway");

		}
		*/

		var mapResult = upnp.AddPortMapping(PORT);
		
		//Debug.Assert(mapResult == (int)Upnp.UpnpResult.Success, "UPNP port mapping is bad " + mapResult);

		GD.Print("shit worked, wild. " + upnp.QueryExternalAddress() );


	}

}
