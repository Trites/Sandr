using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Item {

	public GameObject ProjectilePrefab;

	public override void Use(){
		
		GameObject obj = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity) as GameObject;
		
		Projectile projectile = obj.GetComponent<Projectile>();
		projectile.SetVelocity(new Vector2(1, 0));
		
	}
}
