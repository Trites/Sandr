using UnityEngine;
using System.Collections;

public class MeleeWeapon : MonoBehaviour {

	public LayerMask TargetLayer;
	public float Reach = 2f;
	private Transform _transform;
	
	public void Awake(){
		
		_transform = transform;
	}
	
	public void Strike(){
		
		Debug.DrawRay(_transform.position, Vector2.right * Mathf.Sign(_transform.localScale.x) * Reach, Color.cyan, 0.5f);
		
		RaycastHit2D rayHit = Physics2D.Raycast(_transform.position, Vector2.right * Mathf.Sign(_transform.localScale.x), Reach, TargetLayer);
		
		if(rayHit){
			
			rayHit.collider.gameObject.SendMessage("HitBy", this, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	public void Update(){
		
		if(Input.GetKeyDown(KeyCode.LeftControl)){
			
			Strike();
		}
	}
}
