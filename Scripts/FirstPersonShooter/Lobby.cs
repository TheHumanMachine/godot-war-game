using Godot;
using System;
using System.Collections.Generic;

public partial class Lobby : Node3D
{
	// Called when the node enters the scene tree for the first time.
	[Signal]
    public delegate void OnStartFPSEventHandler();


	private Label connectedCountLabel;
	private PeerNetworkMananger peerNetworkMananger;
	[Export]
	private int count = 0;
	[Export]
	private string labelText = "";
	private int lastKnowCount = 0;
	
	public override void _Ready()
	{
		connectedCountLabel = GetNode<Label>("CanvasLayer/mainContainer/MarginContainer/VBoxContainer/connectedCountLabel");
	}

	public void RegisterPeerNetworkManager( ref PeerNetworkMananger peerNetworkMananger){
		this.peerNetworkMananger = peerNetworkMananger;
	}
	public void RegisterLobbyPlayer(long peerID){
		GD.Print("peerID registered: " + peerID);

		count = peerNetworkMananger.ConnectedList.Count;

		labelText = "connectedPlayers count: " + count;
		if(connectedCountLabel == null){ 
			connectedCountLabel = GetNode<Label>("CanvasLayer/mainContainer/MarginContainer/VBoxContainer/connectedCountLabel");
		}

	}

	private void _on_startbtn_pressed(){
		if(this.IsMultiplayerAuthority()){
			Rpc(nameof(StartFPSSignal));
		}
	}

	[Rpc(CallLocal = true)]
	private void StartFPSSignal(){
		EmitSignal("OnStartFPS");
	}

	
	public override void _Process(double delta)
	{
		if(lastKnowCount != count){
			this.connectedCountLabel.Text = labelText;
			lastKnowCount = count;
		}
	}
}
