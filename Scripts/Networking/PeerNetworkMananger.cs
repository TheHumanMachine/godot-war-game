using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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

    public void SetNetWorkPlayerName(string name, long peerID){
         var result = playerList.Single(s => s.Authority == peerID);
         if(result != null){
            result.Name = name;
            GD.Print("Newly assigned name: " + result.Name);
         }

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

    public INetworkPlayer GetNetworkPlayer(long peerID){
        var result = playerList.Single(s => s.Authority == peerID);
        return result;
    }
    
    private void RegisterConnectedPlayer(long peerID) {
        playerList.Add(new NetworkPlayer(peerID));
        // signal calls something in maingame update NetworkPlayer iwht name, look up object via peerID
        EmitSignal("OnNetworkPlayerAdded", peerID);
    }

    private void UnregisterConnectedPlayer(long peerID) {
		GD.Print("Remove player....");
        var player = GetNodeOrNull(peerID.ToString());
        if (player != null) {
            player.QueueFree();
        }
    }
}
