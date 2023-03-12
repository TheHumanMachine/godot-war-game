public class NetworkPlayer : INetworkPlayer{
    private string name;
    private long authority;
    public NetworkPlayer(long authority){
        Name = name;
        Authority = authority;
    }

    public string Name { get => this.name; set => this.name = value; }
    public long Authority { get => this.authority; init => this.authority = value;}
}