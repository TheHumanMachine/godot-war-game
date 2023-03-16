using Godot;
using System;
using System.Collections.Generic;

public partial class CardGame : Node3D
{


	private List<INetworkPlayer> networkPlayers = new List<INetworkPlayer>();

	private PackedScene CardGamePlayerScene = (PackedScene)GD.Load("res://Scenes/CardGameScenes/CardGamePlayer.tscn");

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void SetNetWorkPlayers(List<INetworkPlayer> players){
		networkPlayers.AddRange(players);
		MakeCardGamePlayers();
	}

	private void MakeCardGamePlayers(){
		int playerNum = 0;

		foreach(var player in networkPlayers){
			GD.Print(playerNum + " is " + player.Authority);
			CardGamePlayer playerControl;
			playerControl = ((CardGamePlayer)CardGamePlayerScene.Instantiate());
			playerControl.Name = player.Authority.ToString();
			
			switch (playerNum)
			{
				case 0:
					playerControl.Position = GetNode<Node3D>("PlayerOnePosition").Position;
					break;
				case 1:
					playerControl.Position = GetNode<Node3D>("PlayerTwoPosition").Position;
					break;
				default:
					playerControl.Position = GetNode<Node3D>("SpectatorPosition").Position;
					break;

			}

			

			GD.Print("Connected player's name..." + playerControl.Name);

			AddChild(playerControl);

			GD.Print("looking at: 0,0,0");
			playerControl.LookAt(new Vector3(0,0,0));
			playerNum++;
		}
	}
}
