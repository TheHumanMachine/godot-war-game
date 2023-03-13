using Godot;
using System;
using System.Collections.Generic;

public partial class Lobby : Node3D
{
	// Called when the node enters the scene tree for the first time.
	private Label connectedCountLabel;
	private Label testLabel;
	private PeerNetworkMananger peerNetworkMananger;
	private int count = 0;
	private bool newRegister = false;
	
	public override void _Ready()
	{
		connectedCountLabel = GetNode<Label>("CanvasLayer/mainContainer/MarginContainer/VBoxContainer/connectedCountLabel");
		testLabel = GetNode<Label>("CanvasLayer/mainContainer/MarginContainer/VBoxContainer/test");
	}

	public void RegisterPeerNetworkManager( ref PeerNetworkMananger peerNetworkMananger){
		this.peerNetworkMananger = peerNetworkMananger;
	}
	public void RegisterLobbyPlayer(long peerID){


		GD.Print("IN PEERNETWORKMANAGER CONENCTION LIST: " + peerNetworkMananger.ConnectedList.Count);


		if(connectedCountLabel == null || testLabel == null){ ///MainMenu/mainMenuContainer/MarginContainer/VBoxContainer/connectedCountLabel
			connectedCountLabel = GetNode<Label>("CanvasLayer/mainContainer/MarginContainer/VBoxContainer/connectedCountLabel");
			testLabel = GetNode<Label>("CanvasLayer/mainContainer/MarginContainer/VBoxContainer/test");// test
		}
		this.connectedCountLabel.Text = "connectedPlayers count: " + peerNetworkMananger.ConnectedList.Count;
		peerNetworkMananger.PrintConnectedPlayers();

	}

	private void _on_startbtn_pressed(){
		GD.Print("In Lobby... player count for peer network mgr: " + peerNetworkMananger.ConnectedList.Count);
	}

	
	public override void _Process(double delta)
	{
		//connectedCountLabel.Text = "connectedPlayers count: " + connectedPlayers.Count;
	}
}
