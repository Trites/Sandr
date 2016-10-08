using UnityEngine;

public class RangedWeapon : Item {

	public GameObject ProjectilePrefab;
	
	private CharacterController2D _controller;


	public override void Use(){
		
		GameObject obj = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity) as GameObject;		
		Projectile projectile = obj.GetComponent<Projectile>();
		
		projectile.SetVelocity(new Vector2(_controller.FacingDirection, 0));		
		_controller.AddForce(new Vector2(10 * -_controller.FacingDirection, 0));
	}
	
	protected override void OnPickUp(){
		
		transform.rotation = Quaternion.identity;
		_controller = GetComponentInParent<CharacterController2D>();
		
		if(_controller == null){
			
			Debug.LogError("Entity " + gameObject.name + " picked up a weapon but does not have a CharacterController2D.");
		}
	}
}
