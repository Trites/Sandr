using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ThrowingWeapon : MonoBehaviour {
	public AudioSource HitSound;
	private Rigidbody2D _body;
	
	protected void Awake(){
		
		_body = GetComponent<Rigidbody2D>();
	}
	
	protected void FixedUpdate(){
		
		if(_body.isKinematic)
			return;
			
		Vector2 vectorToTarget = _body.velocity.normalized * transform.localScale.x;
 		float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
 		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
 		transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 5);	
	}

	protected void OnTriggerEnter2D(Collider2D collider){
					
		HitSound.Play();
		DieOnHit targetHit = collider.GetComponent<DieOnHit>();
		
		if(targetHit != null){
			
			targetHit.HitBy(new MeleeWeapon.WeaponHitData(collider.transform.position, _body.velocity.normalized, 100f));

		}else{
		
				
			_body.isKinematic = true;
			_body.velocity = Vector2.zero;	
		}
	}
}
