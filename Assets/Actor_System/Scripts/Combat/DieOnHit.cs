using UnityEngine;

public class DieOnHit : MonoBehaviour {
	
	public GameObject ParticleEffect;
	
	void Start(){
		
		if(ParticleEffect == null)
			Debug.LogError("No emitter!");
	}
	
	public void HitBy(MeleeWeapon.WeaponHitData hitData){
		
		Vector2 particleVelocity = Vector2.zero;
		
		Rigidbody2D body = GetComponent<Rigidbody2D>();
		if(body != null){
			
			particleVelocity = body.velocity;
		}
		
		DeathParticleEffect emitter = (Instantiate(ParticleEffect, transform.position, Quaternion.identity) as GameObject).GetComponent<DeathParticleEffect>();
		
		emitter.Spawn(hitData.HitPosition, hitData.Direction*10f, particleVelocity*5f, hitData.Force, 120f, 20f, 120);

		Debug.DrawLine(transform.position, hitData.HitPosition, Color.red, 3f);
		Destroy(gameObject);		
	}
}
