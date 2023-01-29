using Godot;
using System;

public partial class map : Node
{
	private PanelContainer mainMenu;
	private LineEdit addressEntry;
	private PackedScene player_scene = (PackedScene)GD.Load("res://Scenes/Player_Controller.tscn");

	const int PORT = 9999;
	private ENetMultiplayerPeer enet_peer = new ENetMultiplayerPeer();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mainMenu = GetNode<PanelContainer>("CanvasLayer/main menu");
		addressEntry = GetNode<LineEdit>("CanvasLayer/main menu/MarginContainer/VBoxContainer/AddressEntry");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_host_button_pressed()
	{
		mainMenu.Visible = false;
		enet_peer.CreateServer(PORT);
		Multiplayer.MultiplayerPeer = enet_peer;
		Multiplayer.PeerConnected += AddPlayer;

		AddPlayer(Multiplayer.GetUniqueId());
	}

	private void _on_join_button_pressed()
	{
		mainMenu.Visible = false;
		enet_peer.CreateClient("localhost", PORT);
		Multiplayer.MultiplayerPeer = enet_peer;

	}

	private void AddPlayer(long peerID) {
		var player = player_scene.Instantiate();
		player.Name = peerID.ToString();
		AddChild(player);
	}
}
