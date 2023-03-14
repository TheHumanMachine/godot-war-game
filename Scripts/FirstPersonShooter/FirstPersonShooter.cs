using Godot;
using System;
using System.Collections.Generic;

public partial class FirstPersonShooter : Node
{

	private PackedScene playerControllerScene = (PackedScene)GD.Load("res://Scenes/Player_Controller.tscn"); //bullet scene here
	private List<INetworkPlayer> networkPlayers = new List<INetworkPlayer>();
	private List<Player_Controller> playerControllers = new List<Player_Controller>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{//HUDLayer/HUD/HealthBar
	}

	// public void OnPlayerHealthChangeHandler(int health){
		
	// 	GD.Print("should be false: " + (healthBar == null));
	// 	healthBar.Value = health;
	// 	GD.Print("ONPLAYERHEALTHCHANGED IS DOING THING..." + health);
	// }


	public void SetNetWorkPlayers(List<INetworkPlayer> players){
		networkPlayers.AddRange(players);
		MakePlayerControllers();
	}

	private void MakePlayerControllers(){
		foreach(var player in networkPlayers){
			Player_Controller playerControl = (Player_Controller)playerControllerScene.Instantiate();
			playerControl.Name = player.Authority.ToString();
			
			
			GD.Print("Connected player's name..." + playerControl.Name);

			GetParent().AddChild(playerControl);

			if (playerControl.IsMultiplayerAuthority()) {
				//playerControl.OnHealthChanged += OnPlayerHealthChangeHandler;
				GD.Print("Adding Player | Multiplayer Authority " + GetMultiplayerAuthority() );
			}
		}
	}
}
