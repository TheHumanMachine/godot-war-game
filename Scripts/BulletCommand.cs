using Godot;
public class BulletCommand {

    private MainGame mainGame;
    private Player_Controller sourcePlayer;
    private projectile_weapon gun;

    private Node3D muzzle_point;
    private PackedScene bulletScene = (PackedScene)GD.Load("res://Scenes/bullet.tscn"); //bullet scene here

    public BulletCommand(MainGame mainGame, Player_Controller sourcePlayer, projectile_weapon gun) {
        this.mainGame = mainGame;
        this.sourcePlayer = sourcePlayer;
        this.gun = gun;
        GD.Print(sourcePlayer.GetMultiplayerAuthority() + " Authority Made Bullet Command");
        muzzle_point = gun.GetNode<Node3D>("gun_model/muzzle_point");
    }

    public void Execute(Vector3 pos) {
        //mainGame.Rpc("SpawnBullet", sourcePlayer, gun, pos, 30, 0.3);
        bullet b = (bullet)bulletScene.Instantiate();
		b.setValues(sourcePlayer, gun, 10, 30);
        b.SetMultiplayerAuthority(sourcePlayer.GetMultiplayerAuthority());
        muzzle_point.AddChild(b,true);
        b.LookAt(pos);
        b.shoot = true;
    }
}  