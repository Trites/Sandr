using UnityEngine;

public class RangedWeapon : Item {

	public GameObject ProjectilePrefab;
	
	private CharacterController2D _controller;

	public override void Use(){
		
		GameObject obj = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity) as GameObject;		
		Projectile projectile = obj.GetComponent<Projectile>();
		
		int direction = transform.rotation.y == 0 ? 1 : -1;
		
		projectile.SetVelocity(new Vector2(direction, 0));
		
		_controller.AddForce(new Vector2(10 * -direction, 0));
	}
	
	protected override void OnPickUp(){
		
		_controller = GetComponentInParent<CharacterController2D>();
		
		if(_controller == null){
			
			Debug.LogError("Entity " + gameObject.name + " picked up a weapon but does not have a CharacterController2D.");
		}
	}
}
