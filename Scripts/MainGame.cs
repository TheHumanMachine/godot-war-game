using Godot;
using System;
using System.Diagnostics;

public partial class MainGame : Node
{
	private PanelContainer mainMenu;
	private LineEdit addressEntry;
	
	private Control hud; 
	private ProgressBar healthbar;

	PeerNetworkMananger peerNetworkManager;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		peerNetworkManager =  GetNode<PeerNetworkMananger>("PeerNetworkMananger");
		
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

		peerNetworkManager.HostServerSetup();
	}

	private void _on_join_button_pressed()
	{
		hud.Visible = true;
		mainMenu.Visible = false;

		peerNetworkManager.OnClientConnectioned(addressEntry.Text);
	}

	private void _on_multiplayer_spawner_spawned(Node node) {
		if(!node.IsClass("Player_Controller")) {
			return;
		}

		Player_Controller pc = (Player_Controller) node;
		if (pc.IsMultiplayerAuthority()) {
			pc.HealthSignal += UpdateHealthBar;
		}
	}
}