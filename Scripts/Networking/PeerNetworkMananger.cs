using Godot;
using System;
using System.Diagnostics;

public partial class PeerNetworkMananger : Node
{

	private PackedScene player_scene = (PackedScene)GD.Load("res://Scenes/Player_Controller.tscn");

    private LineEdit addressEntry;
    const int PORT = 9999;
    private ENetMultiplayerPeer enet_peer;


    public PeerNetworkMananger()
    {
		enet_peer = new ENetMultiplayerPeer();
    }

    public void HostServerSetup(){
        enet_peer.CreateServer(PORT);
        
        Multiplayer.MultiplayerPeer = enet_peer;

        Multiplayer.PeerConnected += RegisterConnectedPlayer;
        Multiplayer.PeerDisconnected += UnregisterConnectedPlayer;
        //Multiplayer.ServerDisconnected += OnServerDisconnect;

        RegisterConnectedPlayer(Multiplayer.GetUniqueId());

        upnpSetup();
    }

    public void OnClientConnectioned(string addressEntry){
        enet_peer.CreateClient(addressEntry, PORT);
        Multiplayer.MultiplayerPeer = enet_peer;
    }


    private void upnpSetup() {

        var upnp = new Upnp();

        var discoverResult = upnp.Discover();

        var mapResult = upnp.AddPortMapping(PORT);

        //GD.Print("upnp Setup executed... " + upnp.QueryExternalAddress());
    }
    
    private void RegisterConnectedPlayer(long peerID) {
        Player_Controller player = (Player_Controller)player_scene.Instantiate();
        player.Name = peerID.ToString();
        GD.Print("Connected player's name..." + player.Name);

		GetParent().AddChild(player);

        if (player.IsMultiplayerAuthority()) {
            //player.HealthSignal += UpdateHealthBar;
            GD.Print("Adding Player | Multiplayer Authority " + GetMultiplayerAuthority() );
        }
    }

    private void UnregisterConnectedPlayer(long peerID) {
		GD.Print("Remove player....");
        var player = GetNodeOrNull(peerID.ToString());
        if (player != null) {
            player.QueueFree();
        }
    }
}
