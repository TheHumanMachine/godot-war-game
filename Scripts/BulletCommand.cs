using Godot;
public class BulletCommand {

    private Player_Controller sourcePlayer;
    private ProjectileWeapon gun;
    private Node3D muzzle_point;
    private PackedScene bulletScene = (PackedScene)GD.Load("res://Scenes/bullet.tscn"); //bullet scene here

    public BulletCommand(Player_Controller sourcePlayer, ProjectileWeapon gun) {
        this.sourcePlayer = sourcePlayer;
        this.gun = gun;
        muzzle_point = gun.GetNode<Node3D>("gun_model/muzzle_point");
    }

    public void Execute(Vector3 pos) {
        
        bullet b = (bullet)bulletScene.Instantiate();
		b.setValues(sourcePlayer, gun, gun.bullet_damage, gun.bullet_speed);
        b.SetMultiplayerAuthority(sourcePlayer.GetMultiplayerAuthority());
        muzzle_point.AddChild(b,true);
        b.LookAt(pos);
        b.shoot = true;
    }
}