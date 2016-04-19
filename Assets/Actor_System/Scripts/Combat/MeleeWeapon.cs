using UnityEngine;
using System.Collections;

public class MeleeWeapon : MonoBehaviour {

	public struct WeaponHitData{
		
		public Vector2 HitPosition { get; private set; }
		public Vector2 Direction{ get; private set; }
		public float Force { get; private set; }
		
		public WeaponHitData(Vector2 hitPosition, Vector2 direction, float force){
			this.HitPosition = hitPosition;
			this.Direction = direction;
			this.Force = force;
		}
	}

	public LayerMask TargetLayer;
	public float Reach = 2f;
	private Transform _transform;
	
	public void Awake(){
		
		_transform = transform;
	}
	
	public void Strike(){
		
		Vector2 position = _transform.position;
		
		Debug.DrawRay(position, Vector2.right * Mathf.Sign(_transform.localScale.x) * Reach, Color.cyan, 0.5f);
		
		RaycastHit2D rayHit = Physics2D.Raycast(position, Vector2.right * Mathf.Sign(_transform.localScale.x), Reach, TargetLayer);
		
		if(rayHit){
			
			rayHit.collider.gameObject.SendMessage("HitBy", new WeaponHitData(rayHit.point, (position - rayHit.point).normalized, 100f), SendMessageOptions.DontRequireReceiver);
		}
	}
	
	public void Update(){
		
		if(Input.GetKeyDown(KeyCode.LeftControl)){
			
			Strike();
		}
	}
}
