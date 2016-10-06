using UnityEngine;

public class RangedWeapon : Item {

	public GameObject ProjectilePrefab;

	public override void Use(){
		
		GameObject obj = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity) as GameObject;		
		Projectile projectile = obj.GetComponent<Projectile>();
		
		int direction = transform.rotation.y == 0 ? 1 : -1;
		
		projectile.SetVelocity(new Vector2(direction, 0));
		
	}
}
