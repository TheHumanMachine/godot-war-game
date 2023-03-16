using Godot;
using System;
using System.Diagnostics;

public partial class MainGame : Node
{
	private PanelContainer mainMenu;
	private LineEdit addressEntry;

	private DebugDisplay debugDisplay;

	private Control hud; 
	private ProgressBar healthbar;

	private PackedScene fpsScene = (PackedScene)GD.Load("res://Scenes/FirstPersonShooter.tscn");
	private PackedScene lobbyScene = (PackedScene)GD.Load("res://Scenes/fps/Lobby.tscn");
	private PackedScene CardGameScene = (PackedScene)GD.Load("res://Scenes/CardGameScenes/CardGame.tscn");

	PeerNetworkMananger peerNetworkManager;

	Node LobbyNode;
	CardGame CardGame;

	Node currentScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		peerNetworkManager =  GetNode<PeerNetworkMananger>("PeerNetworkMananger");
	
		debugDisplay = GetNode<DebugDisplay>("DebugDisplay");

		mainMenu = GetNode<PanelContainer>("MainMenu/mainMenuContainer");
		addressEntry = GetNode<LineEdit>("MainMenu/mainMenuContainer/MarginContainer/VBoxContainer/AddressEntry");

		LobbyNode = lobbyScene.Instantiate();
		
		((Lobby)LobbyNode).RegisterPeerNetworkManager(ref this.peerNetworkManager);

		peerNetworkManager.OnNetworkPlayerAdded += OnNetworkPlayerAdded;
		peerNetworkManager.OnNetworkPlayerAdded += ((Lobby)LobbyNode).RegisterLobbyPlayer;

		//((Lobby)LobbyNode).OnStartFPS += SwitchToFPS;
		((Lobby)LobbyNode).OnStartFPS += SwitchToCardGame;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	//[Rpc(CallLocal = true)]
	public void SwitchToFPS(){
		var fpsNodeScene = (FirstPersonShooter)fpsScene.Instantiate();


		GetTree().Root.RemoveChild(LobbyNode);
		LobbyNode.QueueFree();

		
		GetTree().Root.AddChild(fpsNodeScene);
		GetTree().CurrentScene = fpsNodeScene;
		fpsNodeScene.SetNetWorkPlayers(peerNetworkManager.ConnectedList);
	}

	public void SwitchToCardGame() { 
		CardGame = (CardGame)CardGameScene.Instantiate();
		GD.Print("Switch to card game");

		GetTree().Root.AddChild(CardGame);
		GetTree().Root.RemoveChild(LobbyNode);
		LobbyNode.QueueFree();
		GetTree().CurrentScene = CardGame;


		GD.Print("connected boys: ");
		foreach(INetworkPlayer i in peerNetworkManager.ConnectedList) {
			GD.Print(i.Authority);
		}
		

		CardGame.SetNetWorkPlayers(peerNetworkManager.ConnectedList);
	}

	public void SwitchToScene(Node newScene){
		GetTree().Root.AddChild(newScene);
		GetTree().CurrentScene = newScene;
	}

	public override void _UnhandledInput(InputEvent @event)
	{

		// advanced ai input processing
		if (@event is InputEventKey eventKey){

			// Toggles the debugDisplay
			if (eventKey.Pressed && eventKey.Keycode == Key.F5){
				debugDisplay.Visible = !debugDisplay.Visible;
			}

			// Exits the window
			if (eventKey.Pressed && eventKey.Keycode == Key.Escape){
				GetTree().Quit();
			}
		}

	}

	private void _on_host_button_pressed()
	{
		//hud.Visible = true;
		mainMenu.Visible = false;

		debugDisplay.Visible = true;

		peerNetworkManager.HostServerSetup();
		ChangeToLobbyScene();
	}

	private void ChangeToLobbyScene(){
		currentScene = LobbyNode;
		
		GetTree().Root.AddChild(currentScene);
		GetTree().CurrentScene = currentScene;
	}

	private void _on_join_button_pressed()
	{
		//hud.Visible = true;
		mainMenu.Visible = false;

		debugDisplay.Visible = true;

		peerNetworkManager.OnClientConnectioned(addressEntry.Text);
		ChangeToLobbyScene();
	}


	private void OnNetworkPlayerAdded(long peerID){
		peerNetworkManager.SetNetWorkPlayerName("player" + peerID, peerID);
	}
}