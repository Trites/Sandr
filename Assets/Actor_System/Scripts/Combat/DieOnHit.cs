using UnityEngine;

public class DieOnHit : MonoBehaviour {
	
	public GameObject particleEffect;
	
	void Start(){
		
		if(particleEffect == null)
			Debug.LogError("No emitter!");
	}
	
	public void HitBy(MeleeWeapon.WeaponHitData hitData){
		
		print("Hit!");
		
		DeathParticleEffect emitter = (Instantiate(particleEffect, transform.position, Quaternion.identity) as GameObject).GetComponent<DeathParticleEffect>();
		
		emitter.Spawn(hitData.HitPosition, hitData.Direction*10f, Vector2.zero, hitData.Force, 25f, 5, 50);
			
		Debug.DrawLine(transform.position, hitData.HitPosition, Color.red, 3f);
		Destroy(gameObject);
		
	}
}
