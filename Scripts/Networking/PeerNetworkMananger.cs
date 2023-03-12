using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class PeerNetworkMananger : Node
{

    [Signal]
    public delegate void OnNetworkPlayerAddedEventHandler(long peerID);


    private LineEdit addressEntry;
    const int PORT = 9999;
    private ENetMultiplayerPeer enet_peer;

    private List<INetworkPlayer> playerList = new List<INetworkPlayer>();

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
    }
    
    private void RegisterConnectedPlayer(long peerID) {


        playerList.Add(new NetworkPlayer(peerID));
        // signal calls something in maingame update NetworkPlayer iwht name, look up object via peerID
        EmitSignal("OnNetworkPlayerAdded", peerID);


        // Player_Controller player = (Player_Controller)player_scene.Instantiate();
        // player.Name = peerID.ToString();
        // GD.Print("Connected player's name..." + player.Name);


		//GetParent().AddChild(player);

        // if (player.IsMultiplayerAuthority()) {
        //     //player.HealthSignal += UpdateHealthBar;
        //     GD.Print("Adding Player | Multiplayer Authority " + GetMultiplayerAuthority() );
        // }
    }

    private void UnregisterConnectedPlayer(long peerID) {
		GD.Print("Remove player....");
        var player = GetNodeOrNull(peerID.ToString());
        if (player != null) {
            player.QueueFree();
        }
    }
}
