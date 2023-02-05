using Godot;
public class BulletCommand {

    private MainGame mainGame;
    private Player_Controller sourcePlayer;
    private projectile_weapon gun;

    private PackedScene bullet = (PackedScene)GD.Load("res://Scenes/bullet.tscn"); //bullet scene here

    public BulletCommand(MainGame mainGame, Player_Controller sourcePlayer, projectile_weapon gun) {
        this.mainGame = mainGame;
        this.sourcePlayer = sourcePlayer;
        this.gun = gun;
    }

    public void Execute(Vector3 pos) {
        GD.Print("Packet received shoot");
        mainGame.RpcId(1,"SpawnBullet", sourcePlayer, gun, pos, 30, 0.3);
    }
}