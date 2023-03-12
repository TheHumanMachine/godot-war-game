using Godot;
using System;
using System.Collections.Generic;

public partial class Lobby : Node3D
{
	// Called when the node enters the scene tree for the first time.
	private Label connectedCountLabel;
	private Label testLabel;
	private List<INetworkPlayer> connectedPlayers = new List<INetworkPlayer>();
	private PeerNetworkMananger peerNetworkMananger;
	
	public override void _Ready()
	{
		connectedCountLabel = GetNode<Label>("MainMenu/mainMenuContainer/MarginContainer/VBoxContainer/connectedCountLabel");
		testLabel = GetNode<Label>("test");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void RegisterPeerNetworkManager(PeerNetworkMananger peerNetworkMananger){
		this.peerNetworkMananger = peerNetworkMananger;
	}
	public void RegisterLobbyPlayer(long peerID){
		//connectedPlayers.Add(player);
		GD.Print("Registered in Lobby peer id: " + peerID);
		connectedPlayers.Add(peerNetworkMananger.GetNetworkPlayer(peerID));

		GD.Print("connectedPlayers count: " + connectedPlayers.Count);

		if(connectedCountLabel == null || testLabel == null){ ///MainMenu/mainMenuContainer/MarginContainer/VBoxContainer/connectedCountLabel
			connectedCountLabel = GetNode<Label>("MainMenu/mainMenuContainer/MarginContainer/VBoxContainer/connectedCountLabel");
			testLabel = GetNode<Label>("test");// test
		}


		testLabel.Text = "Chantged in code...";
		connectedCountLabel.Text = "fgsfsd: ";
		GD.Print("label text is: " + connectedCountLabel.Text );
		
	}

	
	public override void _Process(double delta)
	{
	}
}
